namespace UlfenDk.EnergyAssistant.Config;

public class TariffPeriod
{
    public string? Name { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }
}