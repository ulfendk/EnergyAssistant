namespace UlfenDk.EnergyAssistant.Database;

public class CarnotSettings
{
    public long Id { get; set; }

    public bool IsEnabled { get; set; }

    public string User { get; set; }

    public string ApiKey { get; set; }
}