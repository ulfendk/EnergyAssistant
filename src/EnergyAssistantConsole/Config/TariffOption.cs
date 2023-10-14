using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class TariffOption
{
    [YamlMember(Alias = "start_time")]
    public TimeOnly StartTime { get; set; }

    [YamlMember(Alias = "end_time")]
    public TimeOnly EndTime { get; set; }
    
    [YamlMember(Alias = "always_include")]
    public decimal[]? AlwaysInclude { get; set; }

    public decimal[]? Standard { get; set; }

    public decimal[]? Reduced { get; set; }

    [YamlIgnore]
    public decimal CommonCost => AlwaysInclude?.Sum() ?? 0;

    [YamlIgnore]
    public decimal RegularFixedCost => CommonCost + Standard?.Sum() ?? 0;

    [YamlIgnore]
    public decimal ReducedFixedCost => CommonCost + Reduced?.Sum() ?? 0;

}