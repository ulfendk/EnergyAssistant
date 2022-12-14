// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data
open Newtonsoft.Json
open NodaTime

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

let zone = DateTimeZoneProviders.Bcl.GetSystemDefault()
let date value = Instant.FromDateTimeOffset(value).InZone(zone).Date
let localTime (value: TimeSpan) = LocalTime(value.Hours, value.Minutes)

type PeriodCost =
    { StartTime: LocalTime;
      EndTime: LocalTime;
      FixedCost: decimal }

type Tariff =
    { DateInterval: DateInterval;
      Periods: PeriodCost array }

let vat (price: decimal) = (configData.Vat + 1m) * price

type ConfigTariff =
    { StartTimeSpan: TimeSpan;
      EndTimeSpan: TimeSpan;
      FixedCost: decimal }

type ConfigTariffPeriod =
    { StartDateOffset: DateTimeOffset;
      EndDateOffset: DateTimeOffset;
      Periods: ConfigTariff array }

let configTariffs = configData.Tariffs |> Seq.groupBy (fun x -> x.Period) |> Seq.map (fun (k,v) -> (k, v |> Seq.map (fun x -> { StartTimeSpan = x.StartTime; EndTimeSpan = x.EndTime; FixedCost = x.FixedCost }) |> Seq.toArray )) |> dict
let configTariffPeriods = configData.TariffPeriods |> Array.map (fun x ->
    { StartDateOffset = x.StartDate; EndDateOffset = x.EndDate; Periods = configTariffs.Item(x.Name) })

let tariffs = configTariffPeriods |> Array.map (fun x -> //configData.Tariffs
        { DateInterval = DateInterval((date x.StartDateOffset), (date x.EndDateOffset));
          Periods = x.Periods |> Array.map (fun y ->
                { StartTime = localTime y.StartTimeSpan;
                  EndTime = localTime y.EndTimeSpan;
                  FixedCost = y.FixedCost }) })

let activeTariff (dateTime: DateTimeOffset) =
    try
        let theTariff = tariffs |> Array.find (fun x ->
            let theDate = date dateTime
            x.DateInterval.Contains(theDate))
        let theTime = localTime (TimeSpan(dateTime.Hour, dateTime.Minute, 0))
        let theActiveTariff = theTariff.Periods |> Array.find (fun x -> theTime >= x.StartTime && theTime < x.EndTime)
        // printfn "Tariff for found for %O is %O %M" theTime theActiveTariff.StartTime theActiveTariff.FixedCost |> ignore
        Some(theActiveTariff.FixedCost)
    with
        | :? System.Collections.Generic.KeyNotFoundException -> None

let priceWithTariffAndVat (price: decimal) (start : DateTimeOffset) =
    let tariffed =
        match activeTariff start with
        | Some(t) -> price + t
        | None -> price
    // printfn "Tariffed price %M = %M" price tariffed |> ignore
    // printfn "With VAT %M" (vat tariffed) |> ignore
    Math.Round(vat tariffed, 2)

let level (price: decimal) =
    match price with
    | p when p >= configData.Levels.Extreme -> "Extreme"
    | p when p >= configData.Levels.High -> "High"
    | p when p >= configData.Levels.Medium -> "Medium"
    | _ -> "Low"

let hoursOfDay (input: string option) =
    match input with
    | Some(x) -> x.Split('|') |> Array.map (fun x-> x.Trim() |> int)
    | None -> [|0..23|]
    |> Set.ofArray
let spanDefinitions = configData.Spans |> Array.map (fun x -> { Title = x.Title; Duration = x.Hours; MaxHoursInFuture = x.MaxHoursFuture; HoursOfDay = (hoursOfDay x.HoursOfDay)})

[<Literal>]
let energiSample = "../data/energidataservice.json"
let energiUrl = sprintf "https://api.energidataservice.dk/dataset/elspotprices?start=now&sort=HourUTC asc&filter={\"PriceArea\":[\"%s\"]}&limit=48" configData.Carnot.Region
type EnergiDataService = JsonProvider<energiSample>

[<Literal>]
let nordpoolSample = "../data/nordpool.json"
let nordpoolUrl = sprintf "https://www.nordpoolgroup.com/api/marketdata/page/41?currency=,DKK,DKK,EUR&endDate=%s" (DateTime.Now.ToString "dd-MM-yyyy")
type NordpoolService = JsonProvider<nordpoolSample>

// carnot.dk
[<Literal>]
let carnotSample = "../data/carnot.json"
let carnotUrl = sprintf "https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region=%s&daysahead=7" configData.Carnot.Region

type CarnotDk = JsonProvider<carnotSample>

let baseTopic = "energyassistant"
let topic name = sprintf "%s/%s/%s" baseTopic configData.Carnot.Region name

let asPayload obj = JsonConvert.SerializeObject(obj)
let key i = sprintf "span_%i" (i + 1)
let appendTimeKey name = sprintf "%s_time" name
let appendTime name = sprintf "%s Time" name

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

    let levelDiscovery (name: string) (id: string) =
      let topic = topic id
      { Name = name;
        StateTopic = topic;
        JsonAttributeTopic = topic;
        Schema = "json";
        UniqueId = asUniqueId name;
        UnitOfMeasurement = "";
        ValueTemplate = "{{ value_json.level }}" }

    let publish topic payload = Mqtt.publish client (discoveryTopic topic) (asPayload payload)
    publish "spotprice" (priceDiscovery "Spotprice" "spotprice") |> ignore
    publish "spotprice_level" (levelDiscovery "Spotprice Level" "spotprice") |> ignore

    publish "min" (priceDiscovery "Spotprice Minimum" "min") |> ignore
    publish "max" (priceDiscovery "Spotprice Maximum" "max") |> ignore
    publish "avg" (priceDiscovery "Spotprice Average" "avg") |> ignore
    publish "median" (priceDiscovery "Spotprice Median" "median") |> ignore

    publish "min_time" (timestampDiscovery (appendTime "Spotprice Minimum") "min") |> ignore
    publish "max_time" (timestampDiscovery (appendTime "Spotprice Maximum") "max") |> ignore

    spanDefinitions |> Array.iteri (fun i x ->
        let key = key i
        publish key (priceDiscovery x.Title key) |> ignore
        publish (appendTimeKey key) (timestampDiscovery (appendTime x.Title) key) |> ignore)

    client.Dispose |> ignore

    log "Done"


while true do

    let client = Mqtt.connect mqttSettings

    log "Downloading predictions from Carnot.dk..."

    let data = Http.RequestString(carnotUrl,[], [ ("accept", "application/json"); ("apikey", configData.Carnot.ApiKey); ("username", configData.Carnot.User) ])
    let carnotData = CarnotDk.Parse(data)

    log "Downloading spot prices from energidataservice.dk..."

    let energiDataString = Http.RequestString(energiUrl)
    let energiData = EnergiDataService.Parse(energiDataString)

    let name (time : DateTimeOffset) = sprintf "%i-%i" time.Hour (time.Hour + 1)

    let actuals =
        if energiData.Records.Length = 0 then
            log "energidataservice.dk did not return any values, trying Nordpool directly..."
            log "Downloading spot prices from Nordpool..."

            let nordpoolDataString = Http.RequestString(nordpoolUrl)
            let nordpoolData = NordpoolService.Parse(nordpoolDataString)

            nordpoolData.Data.Rows |> Seq.take 24 |> Seq.mapi (fun i x -> //  Seq.map (fun x -> (x.Name, x.Columns.[1].Value)) |> Seq.take 24 |> .toList
              let now = DateTime.UtcNow.Date
              let priceString = x.Columns.[1].Value.String.Value
              let priceString2 = priceString.Replace(',', '.').Replace(" ", "")
              let price = Decimal.Parse(priceString2, System.Globalization.CultureInfo.InvariantCulture)
              let hourDk = now.AddHours(i)///.Add(DateTimeOffset.Now.Offset)
              { Carnot.SegmentPrice.Name = name hourDk;
                Carnot.SegmentPrice.Region = "DK2";
                Carnot.SegmentPrice.Start = hourDk;
                Carnot.SegmentPrice.End = hourDk.Add(TimeSpan.FromHours(1));
                Carnot.SegmentPrice.Value = priceWithTariffAndVat (price / 1000m) hourDk
                Carnot.SegmentPrice.IsPrediction = false }) |> Seq.toList
        else
            energiData.Records |> Seq.map(fun x->
              let hourDk = x.HourUtc;//.Add(DateTimeOffset.Now.Offset)
              { Carnot.SegmentPrice.Name = name hourDk;
                Carnot.SegmentPrice.Region = x.PriceArea;
                Carnot.SegmentPrice.Start = hourDk;
                Carnot.SegmentPrice.End = hourDk.Add(TimeSpan.FromHours(1));
                Carnot.SegmentPrice.Value = priceWithTariffAndVat (x.SpotPriceDkk / 1000m) hourDk
                Carnot.SegmentPrice.IsPrediction = false }) |> Seq.toList

    let carnotPredictions = carnotData.Predictions |> Seq.map(fun x ->
      let hourDk = x.Dktime//.Add(DateTimeOffset.Now.Offset)
      { Carnot.SegmentPrice.Name = name hourDk;
        Carnot.SegmentPrice.Region = x.Pricearea;
        Carnot.SegmentPrice.Start = hourDk;
        Carnot.SegmentPrice.End = hourDk.Add(TimeSpan.FromHours(1));
        Carnot.SegmentPrice.Value = priceWithTariffAndVat (x.Prediction / 1000m) hourDk
        Carnot.SegmentPrice.IsPrediction = true }) |> Seq.toList

    let predictions = actuals @ carnotPredictions |> List.distinctBy (fun x -> x.Start) |> List.sortBy (fun x -> x.Start) |> Array.ofList

    let min = Carnot.min predictions
    let max = Carnot.max predictions
    let avg = Carnot.avg predictions
    let median = Carnot.median predictions

    let now = DateTimeOffset.Now

    // Spans
    let spanAsHours (start : DateTimeOffset) (finish : DateTimeOffset) =
        let rec extractHours (start : DateTimeOffset) (finish : DateTimeOffset) =
            match start with
            | _ when start = finish -> []
            | _ -> start.Hour :: extractHours (start.AddHours(1)) finish
        extractHours start finish |> Set.ofList

    let toPeriods duration = Carnot.getAsSpans predictions duration
    let filterHoursAhead hoursAhead (periods : Carnot.SegmentPrice array) = periods |> Array.filter (fun x -> x.End <= now.AddHours(hoursAhead))
    let filterHoursOfDay hours (periods : Carnot.SegmentPrice array) = periods |> Array.filter (fun x -> Set.isSubset (spanAsHours x.Start x.End) hours)

    let spans = spanDefinitions |> Array.map (fun spanDef ->
        let periods = toPeriods spanDef.Duration |> List.map (fun x -> x |> Array.map (fun y -> { y with Start = y.Start.ToOffset(now.Offset); End = y.End.ToOffset(now.Offset)}))
        let periodsWithinDuration = periods |> List.map (fun x -> filterHoursAhead spanDef.MaxHoursInFuture x)
        let periodsWithinDurationAndTimeOfDay = periodsWithinDuration |> List.map (fun x -> filterHoursOfDay spanDef.HoursOfDay x) |> List.filter (fun x -> x.Length = spanDef.Duration)
        let spans = periodsWithinDurationAndTimeOfDay |> List.map (fun x -> { Title = spanDef.Title; Start = x.[0].Start; Duration = TimeSpan.FromHours spanDef.Duration; Price = x |> Array.averageBy (fun y -> y.Value); Level = x |> Array.averageBy (fun y -> y.Value) |> level })
        let sorted = spans |> List.sortBy (fun x -> x.Price)
        sorted.Head)

    // Prices
    let hourPrices = predictions |> Array.map (fun x -> { Hour = DateTimeOffset(x.Start.Year, x.Start.Month, x.Start.Day, x.Start.Hour, 0, 0, TimeSpan.Zero); Price = x.Value; Level = (level x.Value); IsPrediction = x.IsPrediction }) //fullPrice x.Start fees
    //let currentPrice = hourPrices |> Array.find (fun x -> x.Hour.Date = now.Date && x.Hour.Hour = now.Hour)
    let currentPrice = hourPrices |> Array.find (fun x ->
        let localTime = x.Hour //.Add(DateTimeOffset.Now.Offset)
        let localDateHour = DateTime(localTime.Year, localTime.Month, localTime.Day, localTime.Hour, 0, 0)
        let isMatch = localDateHour.Date = now.Date && localDateHour.Hour = now.Hour
        isMatch)
    let price = { State = currentPrice.Price; Level = level currentPrice.Price; Prices = hourPrices; UpdateAt = now }

    log "Publishing MQTT states..."

    let publish topicPart payload = Mqtt.publish client (topic topicPart) (asPayload payload)
    publish "spotprice" price |> ignore
    publish "min" { State = min.Value; ValidAt = min.Start; UpdatedAt = now } |> ignore
    publish "max" { State = max.Value; ValidAt = max.Start; UpdatedAt = now } |> ignore
    publish "avg" { State = avg; ValidAt = now;  UpdatedAt = now } |> ignore
    publish "median" { State = median; ValidAt = now;  UpdatedAt = now } |> ignore

    spans |> Array.iteri (fun i x ->
        let key = key i
        publish key { State = x.Price; ValidAt = x.Start; UpdatedAt = now } |> ignore)

    client.Dispose()

    log "Done"

    let sleep (time : TimeSpan) = Threading.Thread.Sleep time
    sleep (TimeSpan.FromSeconds 1) |> ignore

    let sleepTimeMinutes = 30 - DateTime.Now.Minute % 30
    let sleetTimeSeconds = 60 - DateTime.Now.Second % 60
    let sleepTime = TimeSpan(0, sleepTimeMinutes, sleetTimeSeconds)

    log (sprintf "Sleeping for %A minutes" sleepTime)
    sleep sleepTime
