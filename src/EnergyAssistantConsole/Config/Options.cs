using System.Reflection;
using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class CarnotOptions
{
    public string? Region { get; set; }

    public string? User { get; set; }

    [JsonPropertyName("api_key")]
    public string? ApiKey { get; set; }
}

public class MqttOptions
{
    public string? Server { get; set; }

    public int? Port { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }

    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("use_tls")]
    public bool? UseTls { get; set; }
}



public class TariffPeriod
{
    public string? Name { get; set; }

    public DateOnly? Start { get; set; }

    public DateOnly? End { get; set; }
}

public class TariffOption
{
    public string? Period { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public float? FixedCost { get; set; }
}

public class ActivitySpan
{
    public string? Title { get; set; }

    public int? Hours { get; set; }

    [JsonPropertyName("max_hours_future")]
    public int? MaxHoursInFuture { get; set; }

    [JsonPropertyName("hours_of_day")]
    public string? HoursOfDay { get; set; }
}

public class Options
{
    public CarnotOptions? Carnot { get; set; }

    public MqttOptions? Mqtt { get; set;}

    public float? Vat { get; set; }

    public TariffPeriod[]? TariffPeriods { get; set; }

    public TariffOption[]? Tariffs { get; set; }

    public Dictionary<string, float>? Levels { get; set; }

    public ActivitySpan[]? Spans { get; set; }
}