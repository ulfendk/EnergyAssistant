namespace UlfenDk.EnergyAssistant.Mqtt;

public record struct HourPrice(
    DateTimeOffset Hour,
    Decimal Price,
    string Level,
    bool IsPrediction);