namespace UlfenDk.EnergyAssistant.Database;

public class FixedFee
{
    public long Id { get; set; }
    
    public required string Name { get; set; }

    public decimal Amount { get; set; }

    public long FeePeriodId { get; set; }
}