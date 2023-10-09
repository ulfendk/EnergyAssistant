using System.Collections.Generic;

namespace UlfenDk.EnergyAssistant.Model;

public record struct SpotPrice
{
    public override int GetHashCode()
    {
        return Hour.GetHashCode();
    }

    public bool Equals(SpotPrice? other)
    {
        return Hour == other?.Hour;
    }

    public string Region { get; set; }

    public DateTimeOffset Hour { get; set; }

    public decimal CalculatedPriceFullTariff { get; set; }

    public decimal CalculatedPriceReducedTariff { get; set; }

    public decimal RawPrice { get; set; }

    public string Level { get; set; }

    public string Source { get; set; }

    public bool IsPrediction { get; set; }
}


public class SpotPriceCollection
{
    private readonly SortedSet<SpotPrice> _prices;

    public SpotPriceCollection()
    {
        _prices = new SortedSet<SpotPrice>();
    }

    public SpotPrice? Current => _prices.SingleOrDefault(x => x.Hour == GetCurrentHour()); 

    public void AddOrUpdateRange(IEnumerable<SpotPrice> prices)
    {
        var today = GetToday();

        var toRemove = _prices.Where(x => x.Hour < GetToday()).ToArray();
        foreach (var expiredPrice in toRemove)
        {
            _prices.Remove(expiredPrice);
        }

        foreach (var newPrice in prices)
        {
            if (_prices.Contains(newPrice))
            {
                _prices.Remove(newPrice);
                _prices.Add(newPrice);
            }
            else
            {
                _prices.Add(newPrice);
            }
        }
    }

    private DateTimeOffset GetCurrentHour()
    {
        var now = DateTimeOffset.Now;
        return new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, 0, 0, now.Offset);
    }

    private DateTimeOffset GetToday()
    {
        var now = DateTimeOffset.Now;
        return new DateTimeOffset(DateTimeOffset.Now.Date, now.Offset);
    }
}