namespace UlfenDk.EnergyAssistant.Model;

public record struct Tariff(decimal fixedCost);

public class TariffCollection
{
    public IDictionary<CalendarInterval, IDictionary<TimeInterval, Tariff>> Tariffs { get;  }

    public TariffCollection(IDictionary<CalendarInterval, IDictionary<TimeInterval, Tariff>> tariffs)
    {
        Tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
    }

    public Tariff this[DateTimeOffset time]
    {
        get
        {
            var tariffPeriod = Tariffs.FirstOrDefault(kv => kv.Key.Contains(time)).Value;
            var tariff = tariffPeriod?.FirstOrDefault(kv => kv.Key.Contains(time)).Value;

            return tariff ?? new(0m);
        }
    }
}
