namespace UlfenDk.EnergyAssistant.Config;

public class ActivityOptions : IOptionsWithDefaults<ActivityOptions>
{
    public ActivitySpan[]? Spans { get; set; }

    public ActivityOptions Default => new ActivityOptions
    {
        Spans = new[]
        {
            new ActivitySpan
            {
                Title = "Laundry",
                HoursDuration = 2,
                CompletedAt = new TimeOnly(7, 0),
                AvoidHours = new []{22, 23}
            }
        }

    };
}