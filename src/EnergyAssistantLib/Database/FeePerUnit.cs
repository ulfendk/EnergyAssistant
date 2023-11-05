namespace UlfenDk.EnergyAssistant.Database;

public class FeePerUnit
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public decimal AmountPerUnit { get; set; }

    public FeeApplication Application { get; set; }

    public long FeePerUnitId { get; set; }
}