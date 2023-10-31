using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class NordpoolOptions : IOptionsWithDefaults<NordpoolOptions>
{
    [YamlMember(Alias = "enabled")]
    public bool IsEnabled { get; set; }

    public NordpoolOptions Default => new NordpoolOptions
    {
        IsEnabled = false
    };
}