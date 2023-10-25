using UlfenDk.EnergyAssistant.Config;

namespace UlfenDk.EnergyAssistant.Model;

public record struct Tariff(decimal RegularCost, decimal ReducedCost);

public class TariffCollection
{
    private Dictionary<CalendarInterval, Dictionary<TimeInterval, Tariff>> Tariffs { get;  }

    public TariffCollection(TariffPeriod[] periods)
    {
        Tariffs = periods.ToDictionary(kv => new CalendarInterval(kv.StartDate, kv.EndDate),
            kv => kv.Daily.ToDictionary(kkv => new TimeInterval(kkv.StartTime, kkv.EndTime),
                kkv => new Tariff(kkv.RegularFixedCost, kkv.ReducedFixedCost)));
    }

    public Tariff this[DateTimeOffset time]
    {
        get
        {
            var tariffPeriod = Tariffs.FirstOrDefault(kv => kv.Key.Contains(time)).Value;
            var tariff = tariffPeriod?.FirstOrDefault(kv => kv.Key.Contains(time)).Value;

            return tariff ?? new();
        }
    }
}
