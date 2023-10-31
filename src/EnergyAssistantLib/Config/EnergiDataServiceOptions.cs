using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class EnergiDataServiceOptions : IOptionsWithDefaults<EnergiDataServiceOptions>
{
    [YamlMember(Alias = "enabled")]
    public bool IsEnabled { get; set; }

    public EnergiDataServiceOptions Default => new EnergiDataServiceOptions
    {
        IsEnabled = true
    };
}