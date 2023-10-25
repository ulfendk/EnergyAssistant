using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.HomeAssistant;

public class Attributes
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("friendly_name")]
    public string? FriendlyName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("device_class")]
    public string? DeviceClass { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("prices")]
    public SpotPriceAttribute[]? Prices { get; init; }
}

public class SpotPriceAttribute
{
    [JsonPropertyName("hour")]
    public DateTimeOffset Hour { get; init; }

    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    [JsonPropertyName("level")]
    public string Level { get; init; }

    [JsonPropertyName("isPrediction")]
    public bool IsPrediction { get; init; }
}

public class SensorPayload<T>
{
    [JsonPropertyName("state")]
    public T State { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("last_updated")]
    public DateTimeOffset? LastUpdated { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("last_changed")]
    public DateTimeOffset? LastChanged { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("attributes")]
    public Attributes? Attributes { get; init; }
}
