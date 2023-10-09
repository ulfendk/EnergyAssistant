using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class MqttOptions
{
    public string? Server { get; set; }

    public int? Port { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }

    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("use_tls")]
    public bool? UseTls { get; set; }

    [JsonPropertyName("allow_untrusted_certificate")]
    public bool? AllowUntrustedCertificate { get; set; }
}