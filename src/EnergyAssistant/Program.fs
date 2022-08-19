// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data

// Command line arguments
let args = System.Environment.GetCommandLineArgs()
let username = args.[1]
let apikey = args.[2]

// Fetching data from carnot.dk
[<Literal>]
let carnotSample = "../data/carnot.json"
let carnotUrl = "https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region=dk2&daysahead=7"

type Carnot = JsonProvider<carnotSample>

let data = Http.RequestString(carnotUrl,[], [ ("accept", "application/json"); ("apikey", apikey); ("username", username) ])
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

let calcAvg (spans : SegmentPrice array list) =
  spans |> List.map (fun s -> (s |> Array.averageBy (fun e -> e.Value), s.[0].Start, s))

let spans = getSpans segments 3 |> calcAvg |> List.sortBy (fun (avg, _, _) -> avg)

printfn "Spans: %A" spans


let min = segments |> Seq.minBy(fun x -> x.Value)
let max = segments |> Seq.maxBy(fun x -> x.Value)

printfn "Min: %A" min
printfn "Max: %A" max

//printfn "%A" (Seq.toList segments)
