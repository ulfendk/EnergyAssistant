namespace UlfenDk.EnergyAssistant.Database;

public class HourlyFeePeriod
{
    public long Id { get; set; }

    public string Name { get; set; }

    public TimeOnly Start { get; set; }

    public List<FeePerUnit> Fees { get; set; }

    public long FeePeriodId { get; set; }
}