namespace UlfenDk.EnergyAssistant.Helpers;

public static class ExtensionMethods
{
    public static DateOnly ToDateOnly(this DateTimeOffset time)
    {
        var date = time.ToLocalTime().LocalDateTime;
        
        return new DateOnly(date.Year, date.Month, date.Day);
    }

    public static DateTimeOffset StartTime(this DateOnly date)
    {
        return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeOffset.Now.Offset);
    }
    
    public static DateTimeOffset EndTime(this DateOnly date)
    {
        return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeOffset.Now.Offset).AddDays(1);
    }
}