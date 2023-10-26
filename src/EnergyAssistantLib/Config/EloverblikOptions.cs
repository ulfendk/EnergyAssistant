using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class EloverblikOptions : IOptionsWithDefaults<EloverblikOptions>
{
    [YamlMember(Alias = "enabled")]
    public bool IsEnabled { get; set; }

    [YamlMember(Alias = "api_key")]
    public string? ApiKey { get; set; }

    [YamlMember(Alias = "metering_point_pid")]
    public string? MeteringPointId { get; set; }

    public EloverblikOptions Default => new EloverblikOptions
    {
        IsEnabled = false,
        ApiKey = default,
        MeteringPointId = default
    };
}