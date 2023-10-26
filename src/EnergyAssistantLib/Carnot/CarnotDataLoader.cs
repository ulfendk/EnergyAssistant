using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergiDataService.Carnot;

public class CarnotDataLoader
{
    public bool IsEnabled { get; }

    private readonly string _user;

    private readonly string _apiKey;

    private readonly string _region;
    private readonly ILogger<CarnotDataLoader> _logger;

    public CarnotDataLoader(OptionsLoader<GeneralOptions> generalOptions, OptionsLoader<CarnotOptions> options, ILogger<CarnotDataLoader> logger) 
    {
        var region = generalOptions.Load().Region;
        var config = options.Load();

        IsEnabled = config.IsEnabled;

        if (IsEnabled)
        {
            _user = config.User ?? throw new ArgumentNullException(nameof(config.User));
            _apiKey = config.ApiKey ?? throw new ArgumentNullException(nameof(config.ApiKey));
            _region = region ?? throw new ArgumentNullException(nameof(GeneralOptions.Region));
        }
        else
        {
            _user = _apiKey = _region = string.Empty;
        }

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SpotPrice[]> GetPredictionsAsync(CancellationToken cancellationToken)
    {
        if (!IsEnabled) throw new InvalidOperationException($"{nameof(CarnotDataLoader)} is not enabled.");
        
        try
        {
            _logger.LogInformation("Downloading predictions from Carnot.dk");
            string uri = $"https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region={_region}&daysahead=7";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", _apiKey);
            client.DefaultRequestHeaders.Add("username", _user);

            var now = DateTimeOffset.Now;
            
            var result = await client.GetFromJsonAsync<CarnotData>(uri, cancellationToken);

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