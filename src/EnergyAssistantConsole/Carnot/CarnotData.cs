namespace UlfenDk.EnergiDataService.Carnot;

public class CarnotDataPrediction
{
    public int? Id { get; set; }

    public string? Pricearea { get; set; }

    public string? Energysource { get; set; }

    public string? Utctime { get; set; }

    public string? Dktime { get; set; }

    public float? Prediction { get; set; }

    public bool? Latest { get; set; }

    public string? Addedtime { get; set; }
}

public class CarnotData
{
    public CarnotDataPrediction[]? Predictions { get; set; }
}
