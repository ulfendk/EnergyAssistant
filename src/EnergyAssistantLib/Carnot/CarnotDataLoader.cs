using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergiDataService.Carnot;

public class CarnotDataLoader
{
    private readonly string _user;

    private readonly string _apiKey;

    private readonly string _region;
    private readonly ILogger<CarnotDataLoader> _logger;

    public CarnotDataLoader(string user, string apiKey, string region, ILogger<CarnotDataLoader> logger)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _region = region ?? throw new ArgumentNullException(nameof(region));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SpotPrice[]> GetPredictionsAsync()
    {
        try
        {
            _logger.LogInformation("Downloading predictions.");
            string uri = $"https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region={_region}&daysahead=7";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", _apiKey);
            client.DefaultRequestHeaders.Add("username", _user);

            var now = DateTimeOffset.Now;
            
            var result = await client.GetFromJsonAsync<CarnotData>(uri);

            var spotPrices = result?.Predictions?.Select(x => new SpotPrice
            {
                Hour = DateTimeOffset.Parse(x.Utctime ?? throw new InvalidDataException(nameof(x.Utctime)))
                    .ToOffset(now.Offset),
                Region = (x.Pricearea ?? _region).ToLowerInvariant(),
                Source = "Carnot.dk",
                RawPrice = (decimal?)x.Prediction / 1000m ?? throw new InvalidDataException(nameof(x.Prediction)),
                IsPrediction = true
            }).ToArray() ?? Array.Empty<SpotPrice>();

            if (spotPrices.Length > 0)
            {
                _logger.LogInformation("Downloaded {count} prices.", spotPrices.Length);
            }
            else
            {
                _logger.LogError("Failed to download any prices.");
            }

            return spotPrices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed with message: {message}.", ex.Message);

            return Array.Empty<SpotPrice>();
        }
    }
}