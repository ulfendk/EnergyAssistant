using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class ActivitySpan
{
    public string? Title { get; set; }

    [YamlMember(Alias = "hours_duration")]
    public int? HoursDuration { get; set; }

    [YamlMember(Alias = "completed_at")]
    public TimeOnly? CompletedAt { get; set; }

    [YamlMember(Alias = "avoid_hours")]
    public int[]? AvoidHours { get; set; }
}