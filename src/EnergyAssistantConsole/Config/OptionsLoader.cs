using System.Text.Json;

namespace UlfenDk.EnergyAssistant.Config;

public static class OptionsLoader
{
    public static Options? GetOptions(string fileName)
    {
        string optionsJson = File.ReadAllText(fileName);

        return JsonSerializer.Deserialize<Options>(optionsJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase,   });
    }
}