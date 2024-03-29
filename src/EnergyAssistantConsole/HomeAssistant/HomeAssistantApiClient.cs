using System.Net;
using System.Net.Http.Json;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.HomeAssistant;

public class HomeAssistantApiClient
{
    private readonly string _url;
    private readonly string _token;
    private readonly string _sensorPrefix;

    public HomeAssistantApiClient(Config.HomeAssistant? config, string region)
    {
        _url = config?.Url ?? "http://homeassistant/core";
        _token = config?.Token ?? Environment.GetEnvironmentVariable("SUPERVISOR_TOKEN")!;
        _sensorPrefix = $"energy_assistant_{region}";
    }

    public async Task<HttpStatusCode> UpdateEntityAsync<T>(string entityType, string entityName, SensorPayload<T> payload)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        var result = await client.PostAsJsonAsync(
            $"{_url}/api/states/{entityType}.{_sensorPrefix}_{entityName}",
            payload);

        return result.StatusCode;
    }
    
    public async Task UpdatePrice(string entityName, string friendlyName, SpotPriceCollection spotPrices, Func<SpotPrice, (decimal Price, string? Level)> selector, DateTimeOffset lastChanged, bool includeLevel = true)
    {
        var now = DateTimeOffset.Now;

        var resultCodes = new List<HttpStatusCode>();

        resultCodes.Add(
            await UpdateEntityAsync("sensor", entityName, new SensorPayload<decimal>
            {
                State = selector(spotPrices.Current).Price,
                LastChanged = lastChanged,
                LastUpdated = now,
                Attributes = new Attributes
                {
                    FriendlyName = friendlyName,
                    DeviceClass = "monetary",
                    Icon = "mdi:cash",
                    UnitOfMeasurement = "DKK/kWh",
                    Prices = spotPrices.Select(x =>
                    {
                        var (price, level) = selector(x);
                        return new SpotPriceAttribute
                        {
                            Hour = x.Hour,
                            Price = price,
                            Level = level,
                            IsPrediction = x.IsPrediction
                        };
                    }).ToArray()
                }
            }));

        if (includeLevel)
        {
            resultCodes.Add(
                await UpdateEntityAsync("sensor", $"{entityName}_level", new SensorPayload<string>
                {
                    State = selector(spotPrices.Current).Level,
                    LastChanged = lastChanged,
                    LastUpdated = now,
                    Attributes = new Attributes
                    {
                        FriendlyName = $"{friendlyName} Level",
                        DeviceClass = "enum",
                        Icon = "mdi:tag",
                    }
                }));
        }

        var failureCodes = resultCodes.Except(new[] { HttpStatusCode.OK, HttpStatusCode.Created }).ToArray();
        if (failureCodes.Any())
        {
            Console.WriteLine($"Failed with {string.Join(", ", failureCodes)}");
        }
    }
}