// For more information see https://aka.ms/fsharp-console-apps
open System
open FSharp.Data

type SegmentPrice = 
  { Name: string;
    Region: string; 
    ValidFrom: DateTimeOffset;
    Value: decimal;
    IsPrediction: bool }

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

let segments = carnotData.Predictions |> Seq.map(fun x -> { Name = sprintf "%i" x.Dktime.Hour ; Region = x.Pricearea; ValidFrom = x.Utctime; Value = x.Prediction / 1000m; IsPrediction = true})

// Calculating data

let toSpan segments =
    segments |> Seq.toList


let min = segments |> Seq.minBy(fun x -> x.Value)
let max = segments |> Seq.maxBy(fun x -> x.Value)

printfn "Min: %A" min
printfn "Max: %A" max

//printfn "%A" (Seq.toList segments)
