namespace UlfenDk.EnergyAssistant.Config;

public class HomeAssistantOptions : IOptionsWithDefaults<HomeAssistantOptions>
{
    public bool IsEnabled { get; set; }
    
    public string Url { get; set; } = "";

    public string Token { get; set; } = "";

    public HomeAssistantOptions Default => new HomeAssistantOptions
    {
        IsEnabled = true,
        Url = "http://homeassistant/core",
        Token = Environment.GetEnvironmentVariable("SUPERVISOR_TOKEN") ?? string.Empty
    };
}