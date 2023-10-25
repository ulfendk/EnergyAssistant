using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class TariffPeriod
{
    [YamlMember(Alias = "start_date")]
    public DateOnly StartDate { get; set; }

    [YamlMember(Alias = "end_date")]
    public DateOnly EndDate { get; set; }

    public TariffOption[]? Daily { get; set; }
}