using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Repository;

namespace UlfenDk.EnergyAssistant.EnergiDataService;

public class EnergiDataServiceLoader
{
    private readonly EnergyAssistantRepository _repository;
    private readonly ILogger<EnergiDataServiceLoader> _logger;

    public async Task<bool> GetIsEnabledAsync()
    {
        var settings = await _repository.GetEnergiDataServiceSettingsAsync();
        
        return settings.IsEnabled;
    }

    public EnergiDataServiceLoader(EnergyAssistantRepository repository, ILogger<EnergiDataServiceLoader> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SpotPrice[]> GetLatestPricesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var generalSettings = await _repository.GetGeneralSettingsAsync();
            var settings = await _repository.GetEnergiDataServiceSettingsAsync();
            
            if (!settings.IsEnabled) throw new InvalidOperationException($"{nameof(EnergiDataServiceLoader)} is not enabled.");
            _logger.LogInformation("Downloading spot prices from EnergiDataService");

            var now = DateTimeOffset.Now;

            string url = $"https://api.energidataservice.dk/dataset/elspotprices?start={now:yyyy-MM-dd}&sort=HourUTC asc&filter={{\"PriceArea\":[\"{generalSettings.Region}\"]}}&limit=48";

            using var client = new HttpClient();
            
            var result = await client.GetFromJsonAsync<EnergiData>(url, cancellationToken);

            var spotPrices = result?.Records?.Select(x => new SpotPrice
                {
                    Hour = new DateTimeOffset(DateTime.Parse(x.HourUtc ?? throw new InvalidDataException(nameof(x.HourUtc))), TimeSpan.Zero)
                        .ToOffset(now.Offset),
                    Region = (x.PriceArea ?? generalSettings.Region).ToLowerInvariant(),
                    RawPrice = (decimal?)x.SpotPriceDkk / 1000m ?? throw new InvalidDataException(nameof(x.SpotPriceDkk)),
                    IsPrediction = false,
                    Source = "EnergiDataService"
                })
                .ToArray() ?? Array.Empty<SpotPrice>();

            if (spotPrices.Length > 0)
            {
                _logger.LogInformation("Downloaded {count} prices.", spotPrices.Length);
            }
            else
            {
                _logger.LogError("No prices downloaded.");
            }

            return spotPrices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed with message: {message}", ex.Message);

            return Array.Empty<SpotPrice>();
        }
    }

}