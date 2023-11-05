using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

[Index(nameof(Hour))]
public class EnergyUsageRecord
{
    public long Id { get; set; }
    public DateTimeOffset Hour { get; set; }
    public required string EntityId { get; set; }
    public required string Source { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    public long FeePeriodId { get; set; }
    public FeePeriod? FeePeriod { get; set; }
}