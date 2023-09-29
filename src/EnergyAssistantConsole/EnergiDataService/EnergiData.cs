using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.EnergiDataService;

public class EnergiDataRecord
{
    [JsonPropertyName("HourUTC")]
    public string? HourUtc {get; set; }

    [JsonPropertyName("HourDK")]
    public string? HourDK { get; set; }

    [JsonPropertyName("PriceArea")]
    public string? PriceArea { get; set;}

    [JsonPropertyName("SpotPriceDKK")]
    public float? SpotPriceDkk { get; set;}

    [JsonPropertyName("SpotPriceEUR")]
    public float? SpotPriceEur { get; set; }
}

public class EnergiData
{
    public int? Total { get; set; }

    public string? Filters { get; set; }

    public string? Sort { get; set; }

    public int? Limit { get; set; }

    public string? Dataset { get; set; }

    public EnergiDataRecord[]? Records { get; set; }
}