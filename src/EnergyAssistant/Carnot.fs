module Carnot

open System

type SegmentPrice = 
  { Name: string;
    Region: string; 
    Start: DateTimeOffset;
    End: DateTimeOffset;
    Value: decimal }

let getAsSpans (segments : array<'a>) spanWidth =
  let rec getSpan (segments : array<'a>) =
    let remaining = segments.Length - spanWidth
    match remaining with
    | x when x >= spanWidth -> segments.[0..(spanWidth-1)] :: (getSpan segments.[1..])
    | _  -> []
  getSpan segments

type Segment = decimal * DateTimeOffset * SegmentPrice array

let calcAvg (spans : SegmentPrice array list) =
  spans |> List.map (fun s -> Segment(s |> Array.averageBy (fun e -> e.Value), s.[0].Start, s))

let min segments = segments |> Seq.minBy(fun x -> x.Value)
let max segments = segments |> Seq.maxBy(fun x -> x.Value)
let avg segments = Math.Round(segments |> Seq.averageBy(fun x -> x.Value), 2)
let median segments =
    let arr = segments |> Seq.sortBy(fun x -> x.Value) |> Array.ofSeq
    arr.[arr.Length / 2].Value
