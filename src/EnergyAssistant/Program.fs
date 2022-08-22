// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data
open MQTTnet
open MQTTnet.Client

// Command line arguments
let args = System.Environment.GetCommandLineArgs()
let configFile = args.[1]

// Configuration
[<Literal>]
let configSample = "../data/options.json"
type Config = JsonProvider<configSample>
let configData = Config.Load(configFile)

let hoursOfDay (input: string) = input.Split('|') |> Array.map (fun x-> x.Trim() |> int) |> Set.ofArray

// MQTT
let client = MqttFactory().CreateMqttClient()
let clientOptionsBuilder =
  MqttClientOptionsBuilder()
    .WithTcpServer(configData.Mqtt.Server, configData.Mqtt.Port)
    .WithCredentials(configData.Mqtt.User, configData.Mqtt.Pwd)
    .WithClientId(configData.Mqtt.ClientId)

if (configData.Mqtt.UseTls) then clientOptionsBuilder.WithTls() |> ignore

let connectionResult = client.ConnectAsync(clientOptionsBuilder.Build()) |> Async.AwaitTask |> Async.RunSynchronously

// let publishResult =
//   client.PublishAsync(MqttApplicationMessageBuilder().WithTopic("/asdf/asdf/asdf").WithPayload("").WithContentType("application/json").Build())
//   |> Async.AwaitTask

// carnot.dk
[<Literal>]
let carnotSample = "../data/carnot.json"
let carnotUrl = sprintf "https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region=%s&daysahead=7" configData.Carnot.Region

type Carnot = JsonProvider<carnotSample>

let data = Http.RequestString(carnotUrl,[], [ ("accept", "application/json"); ("apikey", configData.Carnot.ApiKey); ("username", configData.Carnot.User) ])
let carnotData = Carnot.Parse(data)

type SegmentPrice = 
  { Name: string;
    Region: string; 
    Start: DateTimeOffset;
    End: DateTimeOffset;
    Value: decimal }

let name (time : DateTimeOffset) = sprintf "%i-%i" time.Hour (time.Hour + 1)

let segments = carnotData.Predictions |> Seq.map(fun x -> 
  { Name = name x.Dktime;
    Region = x.Pricearea;
    Start = x.Utctime;
    End = x.Utctime.Add(TimeSpan.FromHours(1));
    Value = x.Prediction / 1000m}) |> Seq.toArray

// Calculating data

let getSpans (segments : array<'a>) spanWidth =
  let rec getSpan (segments : array<'a>) =
    let remaining = segments.Length - spanWidth
    match remaining with
    | x when x >= spanWidth -> segments.[0..(spanWidth-1)] :: (getSpan segments.[1..])
    | _  -> []
  getSpan segments

type Segment = decimal * DateTimeOffset * SegmentPrice array

let calcAvg (spans : SegmentPrice array list) =
  spans |> List.map (fun s -> Segment(s |> Array.averageBy (fun e -> e.Value), s.[0].Start, s))


let spanWidths = configData.Spans |> Array.map (fun x -> x.Hours) |> Set.ofSeq

let spansAsSortedList x = getSpans segments x |> calcAvg |> List.sortBy (fun (avg, _, _) -> avg)

let spansDict = spanWidths |> Set.toSeq |> Seq.map (fun x -> (x, spansAsSortedList x)) |> dict

// let spans = getSpans segments 3 |> calcAvg |> List.sortBy (fun (avg, _, _) -> avg)

let (average, startTime, prices) = spansDict.Item(2).Head

printfn "Spans: %A" spansDict


let min = segments |> Seq.minBy(fun x -> x.Value)
let max = segments |> Seq.maxBy(fun x -> x.Value)
let avg = segments |> Seq.averageBy(fun x -> x.Value)

printfn "Min: %A" min
printfn "Max: %A" max

//printfn "%A" (Seq.toList segments)
