using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Repository;

namespace UlfenDk.EnergyAssistant.Nordpool;

public class NordpoolDataLoader
{
    private readonly EnergyAssistantRepository _repository;
    private readonly ILogger<NordpoolDataLoader> _logger;

    public async Task<bool> GetIsEnabledAsync()
    {
        var settings = await _repository.GetNordPoolSettingsAsync();
        
        return settings.IsEnabled;
    }

    public NordpoolDataLoader(EnergyAssistantRepository repository, ILogger<NordpoolDataLoader> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SpotPrice[]> GetSpotPricesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var generalSettings = await _repository.GetGeneralSettingsAsync();
            var settings = await _repository.GetNordPoolSettingsAsync();
        
            if (!settings.IsEnabled) throw new InvalidOperationException($"{nameof(NordpoolDataLoader)} is not enabled.");
        
            var now = DateTimeOffset.Now;
            
            _logger.LogWarning("Downloading prices. This is a violation of Nordpool terms.");
            
            string uri = $"https://www.nordpoolgroup.com/api/marketdata/page/41?currency=,DKK,DKK,EUR&endDate={now.ToString("dd-MM-yyyy")}";
            using var client = new HttpClient();

            var result = await client.GetFromJsonAsync<Welcome>(uri, Converter.Settings, cancellationToken);

            if (result is null)
            {
                _logger.LogError("Failed with empty response.");
                return Array.Empty<SpotPrice>();
            }

            var spotPrices = result.Data.Rows.Take(24).Select((x, i) =>
            {
                var column = x.Columns.SingleOrDefault(y => y.Name?.ToString().Equals(generalSettings.Region, StringComparison.OrdinalIgnoreCase) ?? false)
                    ?? throw new InvalidDataException("Column not found.");
                var priceString = column.Value.Replace(",", ".").Replace(" ", "");
                var price = decimal.Parse(priceString, CultureInfo.InvariantCulture) / 1000m;
                var hour = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset).AddHours(i);
                return new SpotPrice
                {
                    Hour = hour,
                    Region = (column.Name?.ToString() ?? throw new InvalidDataException(nameof(Column.Name)))
                        .ToLowerInvariant(),
                    IsPrediction = false,
                    Source = "Nordpool",
                    RawPrice = price
                };
            }).ToArray();

            _logger.LogInformation("Downloaded {count} prices.", spotPrices.Length);

            return spotPrices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed with message: {message}.", ex.Message);

            return Array.Empty<SpotPrice>();
        }
    }
}