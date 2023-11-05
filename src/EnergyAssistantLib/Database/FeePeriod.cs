namespace UlfenDk.EnergyAssistant.Database;

public class FeePeriod
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public bool HasElectricHeating { get; set; }

    public DateOnly Start { get; set; }

    public DateOnly End { get; set; }

    // Abonnement
    public List<FixedFee> MonthlyFees { get; set; }

    // Timebaseret
    public List<HourlyFeePeriod> HourlyFees { get; set; }
}