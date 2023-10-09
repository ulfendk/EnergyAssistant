using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class ActivitySpan
{
    public string? Title { get; set; }

    public int? Hours { get; set; }

    [JsonPropertyName("max_hours_future")]
    public int? MaxHoursInFuture { get; set; }

    [JsonPropertyName("hours_of_day")]
    public string? HoursOfDay { get; set; }
}