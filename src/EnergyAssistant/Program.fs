// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data
open Newtonsoft.Json

open Model

// Configuration
let args = System.Environment.GetCommandLineArgs()
let configFile = args.[1]

[<Literal>]
let configSample = "../data/options.json"
type Config = JsonProvider<configSample>
let configData = Config.Load(configFile)

let mqttSettings : Mqtt.MqttSettings =
    { Server = configData.Mqtt.Server;
      Port = configData.Mqtt.Port;
      User = configData.Mqtt.User;
      Password = configData.Mqtt.Pwd;
      ClientId = configData.Mqtt.ClientId;
      UseTls = configData.Mqtt.UseTls }

let fees = 
  { FixedCost = configData.AdditionalCosts.FixedCost;
    OffPeakTariff = configData.AdditionalCosts.RegularTariff;
    PeakTariff = configData.AdditionalCosts.PeakTariff;
    Fee = configData.AdditionalCosts.Fee;
    Vat = configData.AdditionalCosts.Vat }

// carnot.dk
[<Literal>]
let carnotSample = "../data/carnot.json"
let carnotUrl = sprintf "https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region=%s&daysahead=7" configData.Carnot.Region

type CarnotDk = JsonProvider<carnotSample>

let baseTopic = "energyassistant"
let topic name = sprintf "%s/%s/%s" baseTopic configData.Carnot.Region name

let asPayload obj = JsonConvert.SerializeObject(obj)

let publishDiscovery =
    let baseDiscoveryTopic = "homeassistant/sensor"
    let discoveryTopic name = sprintf "%s/%s/%s/config" baseDiscoveryTopic configData.Carnot.Region (asId name)

    let asUniqueId name = asUniqueRegionalId configData.Carnot.Region name

    let client = Mqtt.connect mqttSettings

    printf "Publishing discovery messages..."

    let priceDiscovery (name: string) (id: string) =
      let topic = topic id
      { Name = name;
        StateTopic = topic;
        JsonAttributeTopic = topic;
        Schema = "json";
        UniqueId = asUniqueId name;
        DeviceClass = "monetary";
        UnitOfMeasurement = "DKK";
        ValueTemplate = "{{ value_json.state }}" }

    let timestampDiscovery (name: string) (id: string) =
      let topic = topic id
      { Name = name;
        StateTopic = topic;
        JsonAttributeTopic = topic;
        Schema = "json";
        UniqueId = asUniqueId name;
        DeviceClass = "timestamp";
        UnitOfMeasurement = "";
        ValueTemplate = "{{ value_json.validAt }}" }

    Mqtt.publishResult client (discoveryTopic "spotprice") (asPayload (priceDiscovery "Spotprice" "spotprice")) |> ignore
    Mqtt.publishResult client (discoveryTopic "min") (asPayload (priceDiscovery "Spotprice Minimum" "min")) |> ignore
    Mqtt.publishResult client (discoveryTopic "max") (asPayload (priceDiscovery "Spotprice Maximum" "max")) |> ignore
    Mqtt.publishResult client (discoveryTopic "avg") (asPayload (priceDiscovery "Spotprice Average" "avg")) |> ignore
    Mqtt.publishResult client (discoveryTopic "median") (asPayload (priceDiscovery "Spotprice Median" "median")) |> ignore

    Mqtt.publishResult client (discoveryTopic "min_time") (asPayload (timestampDiscovery "Spotprice Minimum Time" "min")) |> ignore
    Mqtt.publishResult client (discoveryTopic "max_time") (asPayload (timestampDiscovery "Spotprice Maximum Time" "max")) |> ignore

    client.Dispose |> ignore

    printf "Done"


while true do

    let client = Mqtt.connect mqttSettings

    let hoursOfDay (input: string) = input.Split('|') |> Array.map (fun x-> x.Trim() |> int) |> Set.ofArray

    let data = Http.RequestString(carnotUrl,[], [ ("accept", "application/json"); ("apikey", configData.Carnot.ApiKey); ("username", configData.Carnot.User) ])
    let carnotData = CarnotDk.Parse(data)

    let name (time : DateTimeOffset) = sprintf "%i-%i" time.Hour (time.Hour + 1)

    let segments = carnotData.Predictions |> Seq.map(fun x -> 
      { Carnot.SegmentPrice.Name = name x.Dktime;
        Carnot.SegmentPrice.Region = x.Pricearea;
        Carnot.SegmentPrice.Start = x.Utctime;
        Carnot.SegmentPrice.End = x.Utctime.Add(TimeSpan.FromHours(1));
        Carnot.SegmentPrice.Value = fullPrice x.Dktime fees (x.Prediction / 1000m) }) |> Seq.toArray

    let spanWidths = configData.Spans |> Array.map (fun x -> x.Hours) |> Set.ofSeq

    let spansAsSortedList x = Carnot.getSpans segments x |> Carnot.calcAvg |> List.sortBy (fun (avg, _, _) -> avg)

    let spansDict = spanWidths |> Set.toSeq |> Seq.map (fun x -> (x, spansAsSortedList x)) |> dict

    let (average, startTime, prices) = spansDict.Item(2).Head

    // printfn "Spans: %A" spansDict


    let min = Carnot.min segments
    let max = Carnot.max segments
    let avg = Carnot.avg segments
    let median = Carnot.median segments

    let segmentPriceAsPayload (segmentPrice: Carnot.SegmentPrice) = asPayload { State = segmentPrice.Start; Value = segmentPrice.Value }

    let now = DateTimeOffset.Now
    let hourPrices = carnotData.Predictions |> Array.map (fun x -> { Hour = DateTimeOffset(x.Utctime.Year, x.Utctime.Month, x.Utctime.Day, x.Utctime.Hour, 0, 0, now.Offset); Price = fullPrice x.Dktime fees (x.Prediction / 1000m) })
    let currentPrice = hourPrices |> Array.find (fun x -> x.Hour.Date = now.Date && x.Hour.Hour = now.Hour)
    let price = { State = currentPrice.Price; Prices = hourPrices; UpdateAt = now }

    printfn "Publishing MQTT states..."

    Mqtt.publishResult client (topic "spotprice") (asPayload price) |> ignore
    Mqtt.publishResult client (topic "min") (asPayload{ State = min.Value; ValidAt = min.Start; UpdatedAt = now }) |> ignore
    Mqtt.publishResult client (topic "max") (asPayload{ State = max.Value; ValidAt = max.Start; UpdatedAt = now }) |> ignore
    Mqtt.publishResult client (topic "avg") (asPayload{ State = avg; ValidAt = now;  UpdatedAt = now }) |> ignore
    Mqtt.publishResult client (topic "median") (asPayload{ State = median; ValidAt = now;  UpdatedAt = now }) |> ignore

    client.Dispose()

    printfn "Done"


    //printfn "Min: %A" min
    //printfn "Max: %A" max

    let sleepTimeMinutes = 30 - DateTime.Now.Minute % 30
    let sleetTimeSeconds = 60 - DateTime.Now.Second % 60
    let sleepTime = TimeSpan(0, sleepTimeMinutes, sleetTimeSeconds)

    printfn "Sleeping for %A minutes" sleepTime
    Threading.Thread.Sleep(sleepTime)