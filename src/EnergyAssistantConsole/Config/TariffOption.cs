namespace UlfenDk.EnergyAssistant.Config;

public class TariffOption
{
    public string? Period { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public float? FixedCost { get; set; }
}