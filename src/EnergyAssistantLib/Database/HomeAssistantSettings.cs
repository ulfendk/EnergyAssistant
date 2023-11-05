namespace UlfenDk.EnergyAssistant.Database;

public class HomeAssistantSettings
{
    public long Id { get; set; }

    public bool IsEnabled { get; set; }

    public string Url { get; set; }

    public string Token { get; set; }
}