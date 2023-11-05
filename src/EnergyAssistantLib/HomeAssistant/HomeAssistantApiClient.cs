using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Repository;

namespace UlfenDk.EnergyAssistant.HomeAssistant;

public class HomeAssistantApiClient
{
    private readonly HashSet<HttpStatusCode> _successStatusCodes;
    private readonly EnergyAssistantRepository _repository;
    private readonly ILogger<HomeAssistantApiClient> _logger;

    private readonly string _sensorPrefix;

    public HomeAssistantApiClient(EnergyAssistantRepository repository, ILogger<HomeAssistantApiClient> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _sensorPrefix = $"energy_assistant";

        _successStatusCodes = new HashSet<HttpStatusCode>(new[] { HttpStatusCode.OK, HttpStatusCode.Created });
    }

    private async Task<HttpClient> GetHttpClientAsync()
    {
        var settings = await _repository.GetHomeAssistantSettingsAsync();
        string token = string.IsNullOrWhiteSpace(settings.Token)
            ? Environment.GetEnvironmentVariable("SUPERVISOR_TOKEN")!
            : settings.Token;

        var client = new HttpClient()
        {
            BaseAddress = new Uri(settings.Url)
        };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        return client;
    }

    public async Task<HttpStatusCode> UpdateEntityAsync<T>(string entityType, string entityName, SensorPayload<T> payload, CancellationToken cancellationToken)
    {
        string url = $"/api/states/{entityType}.{_sensorPrefix}_{entityName}"; 
        using var client = await GetHttpClientAsync();
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