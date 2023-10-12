namespace UlfenDk.EnergyAssistant.HomeAssistant;

public record struct HourPrice(
    DateTimeOffset Hour,
    Decimal Price,
    string Level,
    bool IsPrediction);