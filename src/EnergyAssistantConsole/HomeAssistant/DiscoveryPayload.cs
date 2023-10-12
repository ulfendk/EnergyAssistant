using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.HomeAssistant;

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