using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class CarnotOptions
{
    public string? User { get; set; }

    [JsonPropertyName("api_key")]
    public string? ApiKey { get; set; }
}