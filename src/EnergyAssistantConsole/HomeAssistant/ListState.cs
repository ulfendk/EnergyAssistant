namespace UlfenDk.EnergyAssistant.HomeAssistant;

public record struct ListState(
    decimal State,
    string Level,
    HourPrice[] Prices,
    DateTimeOffset UpdatedAt);