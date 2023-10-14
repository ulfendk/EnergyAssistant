using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class CarnotOptions
{
    [YamlMember(Alias = "enabled")]
    public bool IsEnabled { get; set; }
    public string? User { get; set; }

    [YamlMember(Alias = "api_key")]
    public string? ApiKey { get; set; }
}