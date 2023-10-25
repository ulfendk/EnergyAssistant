namespace UlfenDk.EnergyAssistant.Config;

public class HomeAssistantOptions : IOptionsWithDefaults<HomeAssistantOptions>
{
    public string? Url { get; set; }

    public string? Token { get; set; }

    public HomeAssistantOptions Default => new HomeAssistantOptions
    {
        Url = null,
        Token = null
    };
}