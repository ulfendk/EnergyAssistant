using System.Reflection;

namespace UlfenDk.EnergyAssistant.Config;

public class Options
{
    public string? Region { get; set; }
    public CarnotOptions? Carnot { get; set; }

    public bool UseNordPoolBackup { get; set; } = false;

    public MqttOptions? Mqtt { get; set;}

    public float? Vat { get; set; }

    public TariffPeriod[]? TariffPeriods { get; set; }

    public TariffOption[]? Tariffs { get; set; }

    public Dictionary<string, float>? Levels { get; set; }

    public ActivitySpan[]? Spans { get; set; }
}