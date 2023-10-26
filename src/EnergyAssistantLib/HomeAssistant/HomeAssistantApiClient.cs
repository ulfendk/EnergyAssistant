using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.HomeAssistant;

public class HomeAssistantApiClient
{
    private readonly HashSet<HttpStatusCode> _successStatusCodes;
    private readonly ILogger<HomeAssistantApiClient> _logger;

    private readonly OptionsLoader<HomeAssistantOptions> _options;
    private readonly string _url;
    private readonly string _token;
    private readonly string _sensorPrefix;

    public HomeAssistantApiClient(OptionsLoader<GeneralOptions> generalOptions, OptionsLoader<HomeAssistantOptions> options, ILogger<HomeAssistantApiClient> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        var region = generalOptions.Load().Region;
        _options = options;
        
        var config = _options.Load();
        
        _url = config.Url ?? "http://homeassistant/core";
        _token = config.Token ?? Environment.GetEnvironmentVariable("SUPERVISOR_TOKEN")!;
        _sensorPrefix = $"energy_assistant_{region}";

        _successStatusCodes = new HashSet<HttpStatusCode>(new[] { HttpStatusCode.OK, HttpStatusCode.Created });
    }

    public async Task<HttpStatusCode> UpdateEntityAsync<T>(string entityType, string entityName, SensorPayload<T> payload, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        string url = $"{_url}/api/states/{entityType}.{_sensorPrefix}_{entityName}"; 
        var result = await client.PostAsJsonAsync(
            url,
            payload,
            cancellationToken);

        if (!_successStatusCodes.Contains(result!.StatusCode))
        {
            _logger.LogError("{method} request to {url} failed with status code {statusCode}", result!.RequestMessage!.Method, url, result.StatusCode);
        }
        
        return result.StatusCode;
    }
    
    public async Task UpdatePriceAsync(string entityName, string friendlyName, SpotPriceCollection spotPrices, Func<SpotPrice, (decimal Price, string? Level)> selector, DateTimeOffset lastChanged, bool includeLevel, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.Now;
        
        _ = await UpdateEntityAsync("sensor", entityName, new SensorPayload<decimal>
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
        },
        cancellationToken);
        
        if (includeLevel)
        {
            _ = await UpdateEntityAsync("sensor", $"{entityName}_level", new SensorPayload<string>
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
            },
            cancellationToken);
        }
    }
}