using System.Text.Json.Serialization;

namespace UlfenDk.EnergyAssistant.Eloverblik;

public class ElOverblikData
{
    [JsonPropertyName("result")]
    public ElOverblikResult[] Results;
}

public class ElOverblikResult
{
    [JsonPropertyName("success")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("errorText")]
    public string ErrorText { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("stackTrace")]
    public string StackTrace { get; set; }

    [JsonPropertyName("MyEnergyData_MarketDocument")]
    public ElOverblikMarketDocument MarketDocument { get; set; }
}

public class ElOverblikMarketDocument
{
    [JsonPropertyName("mRID")]
    public string MRid { get; set; }

    [JsonPropertyName("createdDateTime")]
    public string CreatedDateTime { get; set; }

    [JsonPropertyName("sender_MarketParticipant.name")]
    public string ParticipantName { get; set; }

    [JsonPropertyName("sender_MarketParticipant.mRID")]
    public MRid ParticipantMRid { get; set; }

    [JsonPropertyName("period.timeInterval")]
    public TimeInterval Interval { get; set; }

    [JsonPropertyName("TimeSeries")]
    public TimeSeriesEntry[] TimeSeries { get; set; }

}

public class MRid
{
    [JsonPropertyName("codingScheme")]
    public string CodingScheme { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class TimeInterval
{
    [JsonPropertyName("start")]
    public string Start { get; set; }

    [JsonPropertyName("end")]
    public string End { get; set; }
}

public class TimeSeriesEntry
{
    [JsonPropertyName("mRID")]
    public string MRid { get; set; }

    [JsonPropertyName("businessType")]
    public string BusinessType { get; set; }

    [JsonPropertyName("curveType")]
    public string CurveType { get; set; }

    [JsonPropertyName("measurement_Unit.name")]
    public string UnitOfMeasure { get; set; }

    [JsonPropertyName("MarketEvaluationPoint")]
    public EvaluationPoint MarketEvaluationPoint { get; set; }

    [JsonPropertyName("Period")]
    public PeriodEntry[] Periods { get; set; }
}

public class EvaluationPoint
{
    [JsonPropertyName("mRID")]
    public MRid MRid { get; set; }
}

public class PeriodEntry
{
    [JsonPropertyName("resolution")]
    public string Resolution { get; set; }

    [JsonPropertyName("timeInterval")]
    public TimeInterval Interval { get; set; }

    [JsonPropertyName("point")]
    public PointEntry[] Points { get; set; }
}

public class PointEntry
{
    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("out_Quantity.quantity")]
    public string Quantity { get; set; }

    [JsonPropertyName("out_Quantity.quality")]
    public string Quality { get; set; }
}