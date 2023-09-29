using System.Collections.Generic;

namespace UlfenDk.EnergyAssistant.Model;

public record struct potPrice
{
    override GetHash()
    {
        // Use only Time for this.
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
    private readonly SortedHashset<SpotPrice> _prices;

    public SpotPriceCollection()
    {
        _prices = new SortedHashset<SpotPrice>(x => x.Hour);
    }

    public SpotPrice Current => _prices.TryGet(GetCurrentHour(), out var value) ? value : default; 

    public void AddOrUpdateRange(IEnumerable<SpotPrice> prices)
    {
        var today = GetToday();

        var toRemove = _prices.Where(x => x.Hour < startOfDay).ToArray();
        foreach (var expiredPrice in toRemove)
        {
            _prices.Remove(expiredPrice);
        }

        foreach (var newPrice in prices)
        {
            if (_prices.Contains(newPrice))
            {
                _prices.Update(newPrice);
            }
            else
            {
                _prices.Add(newPrice);
            }
        }
    }

    private DateTimeOffset GetCurrentHour() => null;

    private DateTimeOffset GetToday() => null;
}