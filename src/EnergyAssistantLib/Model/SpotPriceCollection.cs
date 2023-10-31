using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace UlfenDk.EnergyAssistant.Model;

public class SpotPriceCollection : IEnumerable<SpotPrice>
{
    private class RecordComparer : IEqualityComparer<SpotPrice>, IComparer<SpotPrice>
    {
        public bool Equals(SpotPrice x, SpotPrice y) => x.Equals(y);

        public int GetHashCode(SpotPrice obj) => obj.GetHashCode();

        public int Compare(SpotPrice? x, SpotPrice? y) => (x, y) switch
        {
            ({ } a, { } b)  => a.CompareTo(b),
            _ => -1
        };
    }

    private readonly object _lock = new object();

    private readonly SortedSet<SpotPrice> _prices;

    public SpotPriceCollection()
    {
        _prices = new SortedSet<SpotPrice>(new RecordComparer());
        // _prices = new SortedSet<SpotPrice>(new RecordComparer());
    }

    public SpotPrice? Current
    {
        get
        {
            lock (_lock)
            {
                return _prices.SingleOrDefault(x => x.Hour == GetCurrentHour());
            }; 
        }
    }


    public void AddOrUpdateRange(IEnumerable<SpotPrice> prices)
    {
        var today = GetToday();

        lock (_lock)
        {
            var toRemove = _prices.Where(x => x.Hour < today).ToArray();
            foreach (var expiredPrice in toRemove)
            {
                _prices.Remove(expiredPrice);
            }

            foreach (var newPrice in prices)
            {
                if (!_prices.Add(newPrice))
                {
                    _prices.Remove(newPrice);
                    _prices.Add(newPrice);
                }
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

    public IEnumerator<SpotPrice> GetEnumerator() => _prices.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _prices.GetEnumerator();
}