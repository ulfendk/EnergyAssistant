namespace UlfenDk.EnergyAssistant.Model;

public record struct CalendarInterval(DateOnly Start, DateOnly End)
{
    public bool Contains(DateTimeOffset value)
    {
        var date = new DateOnly(value.Year, value.Month, value.Day);

        return date >= Start && date < End;
    }
}
