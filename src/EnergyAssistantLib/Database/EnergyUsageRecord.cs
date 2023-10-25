using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

[Index(nameof(Hour))]
public class EnergyUsageRecord
{
    public long EnergyUsageRecordId { get; set; }
    public DateTimeOffset Hour { get; set; }
    public string? EntityId { get; set; }
    public required string Source { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}