module Model

open System
open Newtonsoft.Json
open NodaTime

type Payload =
  { [<JsonProperty("state")>]State: DateTimeOffset;
    [<JsonProperty("value")>]Value: decimal }

type NumericState =
  { [<JsonProperty("state")>]State: decimal;
    [<JsonProperty("validAt")>]ValidAt: DateTimeOffset
    [<JsonProperty("updatedAt")>]UpdatedAt: DateTimeOffset }

type HourPrice =
  { [<JsonProperty("hour")>] Hour: DateTimeOffset;
    [<JsonProperty("price")>]Price: decimal;
    [<JsonProperty("level")>]Level: string}

type ListState =
  { [<JsonProperty("state")>]State: decimal;
    [<JsonProperty("level")>]Level: string;
    [<JsonProperty("prices")>]Prices: HourPrice array;
    [<JsonProperty("updatedAt")>]UpdateAt: DateTimeOffset }

type DiscoveryPayload =
    { [<JsonProperty("name")>]Name: string;
      [<JsonProperty("state_topic")>]StateTopic: string;
      [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
      [<JsonProperty("schema")>]Schema: string;
      [<JsonProperty("unique_id")>]UniqueId: string;
      [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
      [<JsonProperty("value_template")>]ValueTemplate: string }

type DiscoveryPayloadWithDeviceClass =
    { [<JsonProperty("name")>]Name: string;
      [<JsonProperty("state_topic")>]StateTopic: string;
      [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
      [<JsonProperty("schema")>]Schema: string;
      [<JsonProperty("unique_id")>]UniqueId: string;
      [<JsonProperty("device_class")>]DeviceClass: string;
      [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
      [<JsonProperty("value_template")>]ValueTemplate: string }

let asId name =
  let regex = System.Text.RegularExpressions.Regex("[a-zA-Z0-9_-]")
  let getChar chr =
    match chr with
    | chr when regex.IsMatch(string chr) -> chr
    | _ -> '_'
  name |> Seq.map(fun x -> getChar x) |> Array.ofSeq |> System.String

let asUniqueRegionalId region name =
  sprintf "EnergyAssistant_%s_%s" region name

type TariffPeriod =
  { StartDate: LocalDate;
    EndDate: LocalDate }

type Tariffs =
  { Fixed: decimal;
    Vat: decimal }

type SpanDefinition =
    { Title: string;
      Duration: int;
      MaxHoursInFuture: int;
      HoursOfDay: int Set }

type Span =
  { Title: string;
    Start: DateTimeOffset; 
    Duration: TimeSpan;
    //HoursCovered: int Set;
    Price: decimal;
    Level: string }

// let zone = DateTimeZoneProviders.Bcl.GetSystemDefault()
// let currentDate = SystemClock.Instance.GetCurrentInstant().InZone(zone).Date
// let xxx = NodaTime.Period.Between(currentDate, currentDate)