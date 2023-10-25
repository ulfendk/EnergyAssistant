using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

[Index(nameof(Hour))]
public class SpotPriceRecord
{
    public long SpotPriceRecordId { get; set; }
    public DateTimeOffset Hour { get; set; }
    public decimal RawPrice { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal ReducedPrice { get; set; }
    public bool IsPrediction { get; set; }

    public required string Region { get; set; }
    public required string Source { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}