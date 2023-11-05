using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

[Index(nameof(Hour))]
public class SpotPriceRecord
{
    public long Id { get; set; }
    public DateTimeOffset Hour { get; set; }

    public required string Region { get; set; }
    
    public required string Source { get; set; }

    public decimal RawPrice { get; set; }
    
    public bool IsPrediction { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
    
    public long? FeePeriodId { get; set; }
    
    public FeePeriod? FeePeriod { get; set; }
}