using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Mqtt;

// public class Payload
// {
// // type Payload =
// //   { [<JsonProperty("state")>]State: DateTimeOffset;
// //     [<JsonProperty("value")>]Value: decimal }
//
// }
//
// public class NumericState
// {
// // type NumericState =
// //   { [<JsonProperty("state")>]State: decimal;
// //     [<JsonProperty("validAt")>]ValidAt: DateTimeOffset
// //     [<JsonProperty("updatedAt")>]UpdatedAt: DateTimeOffset }
//
// }

// {
// // type HourPrice =
// //   { [<JsonProperty("hour")>] Hour: DateTimeOffset;
// //     [<JsonProperty("price")>]Price: decimal;
// //     [<JsonProperty("level")>]Level: string
// //     [<JsonProperty("isPrediction")>]IsPrediction: bool}
// }

// public class ListState
// {
// // type ListState =
// //   { [<JsonProperty("state")>]State: decimal;
// //     [<JsonProperty("level")>]Level: string;
// //     [<JsonProperty("prices")>]Prices: HourPrice array;
// //     [<JsonProperty("updatedAt")>]UpdateAt: DateTimeOffset }

public class DiscoveryPayload
{
    public required string Name { get; init; }
    
    [JsonPropertyName("state_topic")]
    public required string StateTopic { get; init; }

    [JsonPropertyName("command_topic")]
    public required string CommandTopic { get; init; }

    [JsonPropertyName("json_attributes_topic")]
    public required string JsonAttributesTopic { get; init; }

    public required string Schema { get; init; }

    public required string UniqueId { get; init; }

    [JsonPropertyName("value_template")]
    public required string ValueTemplate { get; init; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; init; }

    [JsonPropertyName("device_class")]
    public string? DeviceClass { get; init; }
}
// }
// public class DiscoveryPayload
// {
// // type DiscoveryPayload =
// //     { [<JsonProperty("name")>]Name: string;
// //       [<JsonProperty("state_topic")>]StateTopic: string;
// //       [<JsonProperty("command_topic")>]CommandTopic: string;
// //       [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
// //       [<JsonProperty("schema")>]Schema: string;
// //       [<JsonProperty("unique_id")>]UniqueId: string;
// //       [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
// //       [<JsonProperty("value_template")>]ValueTemplate: string }
//
// // type DiscoveryPayloadWithDeviceClass =
// //     { [<JsonProperty("name")>]Name: string;
// //       [<JsonProperty("state_topic")>]StateTopic: string;
// //       [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
// //       [<JsonProperty("schema")>]Schema: string;
// //       [<JsonProperty("unique_id")>]UniqueId: string;
// //       [<JsonProperty("device_class")>]DeviceClass: string;
// //       [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
// //       [<JsonProperty("value_template")>]ValueTemplate: string }
//
// }