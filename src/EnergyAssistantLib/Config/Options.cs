using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class Options
{
    public string? Region { get; set; }

    [YamlMember(Alias = "homeassistant")]
    public HomeAssistantOptions? HomeAssistant { get; set; }

    public CarnotOptions? Carnot { get; set; }

    [YamlMember(Alias = "use_nordpool_backup")]
    public bool UseNordPoolBackup { get; set; } = false;

    public float? Vat { get; set; }

    [YamlMember(Alias = "tariffs")]
    public TariffPeriod[]? TariffPeriods { get; set; }

    public Dictionary<string, float>? Levels { get; set; }

    public ActivitySpan[]? Spans { get; set; }
}