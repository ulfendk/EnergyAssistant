namespace UlfenDk.EnergyAssistant.Mqtt;

public class Payload
{
// type Payload =
//   { [<JsonProperty("state")>]State: DateTimeOffset;
//     [<JsonProperty("value")>]Value: decimal }

}

public class NumericState
{
// type NumericState =
//   { [<JsonProperty("state")>]State: decimal;
//     [<JsonProperty("validAt")>]ValidAt: DateTimeOffset
//     [<JsonProperty("updatedAt")>]UpdatedAt: DateTimeOffset }

}

public class HourPrice
{
// type HourPrice =
//   { [<JsonProperty("hour")>] Hour: DateTimeOffset;
//     [<JsonProperty("price")>]Price: decimal;
//     [<JsonProperty("level")>]Level: string
//     [<JsonProperty("isPrediction")>]IsPrediction: bool}
}

public class ListState
{
// type ListState =
//   { [<JsonProperty("state")>]State: decimal;
//     [<JsonProperty("level")>]Level: string;
//     [<JsonProperty("prices")>]Prices: HourPrice array;
//     [<JsonProperty("updatedAt")>]UpdateAt: DateTimeOffset }

}
public class DiscoveryPayload
{
// type DiscoveryPayload =
//     { [<JsonProperty("name")>]Name: string;
//       [<JsonProperty("state_topic")>]StateTopic: string;
//       [<JsonProperty("command_topic")>]CommandTopic: string;
//       [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
//       [<JsonProperty("schema")>]Schema: string;
//       [<JsonProperty("unique_id")>]UniqueId: string;
//       [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
//       [<JsonProperty("value_template")>]ValueTemplate: string }

// type DiscoveryPayloadWithDeviceClass =
//     { [<JsonProperty("name")>]Name: string;
//       [<JsonProperty("state_topic")>]StateTopic: string;
//       [<JsonProperty("json_attributes_topic")>]JsonAttributeTopic: string;
//       [<JsonProperty("schema")>]Schema: string;
//       [<JsonProperty("unique_id")>]UniqueId: string;
//       [<JsonProperty("device_class")>]DeviceClass: string;
//       [<JsonProperty("unit_of_measurement")>]UnitOfMeasurement: string;
//       [<JsonProperty("value_template")>]ValueTemplate: string }

}