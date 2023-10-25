namespace UlfenDk.EnergyAssistant.Model;

public record struct TimeInterval(TimeOnly Start, TimeOnly End)
{
    public bool Contains(DateTimeOffset value)
    {
        var localTime = value.ToLocalTime();
        var time = new TimeOnly(localTime.Hour, localTime.Minute, localTime.Second);

        return time >= Start && time < End;
    }
}
