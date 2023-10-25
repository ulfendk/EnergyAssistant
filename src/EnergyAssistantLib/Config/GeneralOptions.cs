using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class GeneralOptions : IOptionsWithDefaults<GeneralOptions>
{
    public string? Region { get; set; }

    [YamlMember(Alias = "use_nordpool_backup")]
    public bool UseNordPoolBackup { get; set; } = false;
    
    public GeneralOptions Default => new GeneralOptions
    {
        Region = "dk2",
        UseNordPoolBackup = false
    };
}