// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data
open Newtonsoft.Json

open Model

// Configuration
let args = System.Environment.GetCommandLineArgs()
let configFile = args.[1]

let log txt = printf "[%s] %s%s" (DateTime.Now.ToString "yyyy-MM-dd HH:mm:ss") txt Environment.NewLine

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

let hoursOfDay (input: string option) =
    match input with
    | Some(x) -> x.Split('|') |> Array.map (fun x-> x.Trim() |> int) |> Set.ofArray |> Seq.sort |> Array.ofSeq
    | None -> [|0..23|]

let spans = configData.Spans |> Array.map (fun x -> { Title = x.Title; Duration = x.Hours; MaxHoursInFuture = x.MaxHoursFuture; HoursOfDay = (hoursOfDay x.HoursOfDay)})

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

    log "Publishing discovery messages..."

    let priceDiscovery (name: string) (id: string) =
      let topic = topic id
      { Name = name;
        StateTopic = topic;
        JsonAttributeTopic = topic;
        Schema = "json";
        UniqueId = asUniqueId name;
        DeviceClass = "monetary";
        UnitOfMeasurement = "DKK/kWh";
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

    let publish topic payload = Mqtt.publish client (discoveryTopic topic) (asPayload payload)
    publish "spotprice" (priceDiscovery "Spotprice" "spotprice") |> ignore
    publish "spotprice" (priceDiscovery "Spotprice" "spotprice") |> ignore
    publish "min" (priceDiscovery "Spotprice Minimum" "min") |> ignore
    publish "max" (priceDiscovery "Spotprice Maximum" "max") |> ignore
    publish "avg" (priceDiscovery "Spotprice Average" "avg") |> ignore
    publish "median" (priceDiscovery "Spotprice Median" "median") |> ignore

    publish "min_time" (timestampDiscovery "Spotprice Minimum Time" "min") |> ignore
    publish "max_time" (timestampDiscovery "Spotprice Maximum Time" "max") |> ignore

    client.Dispose |> ignore

    log "Done"


while true do

    let client = Mqtt.connect mqttSettings

    let data = Http.RequestString(carnotUrl,[], [ ("accept", "application/json"); ("apikey", configData.Carnot.ApiKey); ("username", configData.Carnot.User) ])
    let carnotData = CarnotDk.Parse(data)

    let name (time : DateTimeOffset) = sprintf "%i-%i" time.Hour (time.Hour + 1)

    let predictions = carnotData.Predictions |> Seq.map(fun x -> 
      { Carnot.SegmentPrice.Name = name x.Dktime;
        Carnot.SegmentPrice.Region = x.Pricearea;
        Carnot.SegmentPrice.Start = x.Utctime;
        Carnot.SegmentPrice.End = x.Utctime.Add(TimeSpan.FromHours(1));
        Carnot.SegmentPrice.Value = fullPrice x.Dktime fees (x.Prediction / 1000m) }) |> Seq.toArray

    let min = Carnot.min predictions
    let max = Carnot.max predictions
    let avg = Carnot.avg predictions
    let median = Carnot.median predictions

    let now = DateTimeOffset.Now
    let hourPrices = predictions |> Array.map (fun x -> { Hour = DateTimeOffset(x.Start.Year, x.Start.Month, x.Start.Day, x.Start.Hour, 0, 0, now.Offset); Price = fullPrice x.Start fees x.Value })
    let currentPrice = hourPrices |> Array.find (fun x -> x.Hour.Date = now.Date && x.Hour.Hour = now.Hour)
    let price = { State = currentPrice.Price; Prices = hourPrices; UpdateAt = now }

    //let hourPrices = carnotData.Predictions |> Array.map (fun x -> { Hour = DateTimeOffset(x.Utctime.Year, x.Utctime.Month, x.Utctime.Day, x.Utctime.Hour, 0, 0, now.Offset); Price = fullPrice x.Dktime fees (x.Prediction / 1000m) })

    // INCLUDE config span data in the below and create a new type based on the collected span (take first only) and the config data
    let getDataAsSpans width = Carnot.getAsSpans predictions width |> List.map (fun x -> { Start = (x |> Array.head).Start; Duration = TimeSpan.FromHours x.Length; HoursCovered = (x |> Array.map (fun y -> y.Start.Hour) |> Set.ofArray); Price = x |> Array.averageBy (fun y -> y.Value)  })
    let spansWithData = spans |> Array.map (fun x -> (x, (getDataAsSpans x.Duration |> List.map (fun x -> ( x |> Array.averageBy (fun x -> x.Value), x))))) |> Array.map (fun (x, ys) -> (x, ys |> List.sortBy (fun (x, ys) -> x))) 

    printf "%A" spansWithData

    let spanWidths = configData.Spans |> Array.map (fun x -> x.Hours) |> Set.ofSeq
    let spansAsSortedList x = Carnot.getAsSpans predictions x |> Carnot.calcAvg |> List.sortBy (fun (avg, _, _) -> avg)

    //let spansDict = spanWidths |> Set.toSeq |> Seq.map (fun x -> (x, spansAsSortedList x)) |> dict
    //let (average, startTime, prices) =
    //    match spansDict with
    //    | x when x.Count > 0 -> spansDict.Item(2).Head
    //    | _ -> 




    log "Publishing MQTT states..."

    let publish topicPart payload = Mqtt.publish client (topic topicPart) (asPayload payload)
    publish "spotprice" price |> ignore
    publish "min" { State = min.Value; ValidAt = min.Start; UpdatedAt = now } |> ignore
    publish "max" { State = max.Value; ValidAt = max.Start; UpdatedAt = now } |> ignore
    publish "avg" { State = avg; ValidAt = now;  UpdatedAt = now } |> ignore
    publish "median" { State = median; ValidAt = now;  UpdatedAt = now } |> ignore

    client.Dispose()

    log "Done"

    let sleep (time : TimeSpan) = Threading.Thread.Sleep time
    sleep (TimeSpan.FromSeconds 1) |> ignore

    let sleepTimeMinutes = 30 - DateTime.Now.Minute % 30
    let sleetTimeSeconds = 60 - DateTime.Now.Second % 60
    let sleepTime = TimeSpan(0, sleepTimeMinutes, sleetTimeSeconds)

    log (sprintf "Sleeping for %A minutes" sleepTime)
    sleep sleepTime