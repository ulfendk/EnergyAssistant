using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class CarnotOptions: IOptionsWithDefaults<CarnotOptions>
{
    [YamlMember(Alias = "enabled")]
    public bool IsEnabled { get; set; }
    public string? User { get; set; }

    [YamlMember(Alias = "api_key")]
    public string? ApiKey { get; set; }

    public CarnotOptions Default => new CarnotOptions
    {
        IsEnabled = false,
        User = null,
        ApiKey = null
    };
}