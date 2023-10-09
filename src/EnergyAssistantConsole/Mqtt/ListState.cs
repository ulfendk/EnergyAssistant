namespace UlfenDk.EnergyAssistant.Mqtt;

public record struct ListState(
    decimal State,
    string Level,
    HourPrice[] Prices,
    DateTimeOffset UpdatedAt);