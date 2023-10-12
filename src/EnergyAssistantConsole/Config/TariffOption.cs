namespace UlfenDk.EnergyAssistant.Config;

public class TariffOption
{
    public string? Period { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }
    
    public float[]? CommonCosts { get; set; }

    public float[]? RegularAdditionalCosts { get; set; }

    public float[]? ReducedAdditionalCosts { get; set; }

    public float CommonCost => CommonCosts?.Sum() ?? 0f;
    public float RegularFixedCost => CommonCost + RegularAdditionalCosts?.Sum() ?? 0;

    public float ReducedFixedCost => CommonCost + ReducedAdditionalCosts?.Sum() ?? 0;

}