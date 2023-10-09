using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class CarnotOptions
{
    public string? Region { get; set; }

    public string? User { get; set; }

    [JsonPropertyName("api_key")]
    public string? ApiKey { get; set; }
}