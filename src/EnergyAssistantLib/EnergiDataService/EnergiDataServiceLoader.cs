using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.EnergiDataService;

public class EnergiDataServiceLoader
{
    private readonly string _region;
    private readonly ILogger<EnergiDataServiceLoader> _logger;

    public EnergiDataServiceLoader(OptionsLoader<GeneralOptions> options, ILogger<EnergiDataServiceLoader> logger)
    {
        var config = options.Load();
        
        _region = config.Region ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SpotPrice[]> GetLatestPricesAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Downloading spot prices from EnergiDataService");

            var now = DateTimeOffset.Now;

            string url = $"https://api.energidataservice.dk/dataset/elspotprices?start={now:yyyy-MM-dd}&sort=HourUTC asc&filter={{\"PriceArea\":[\"{_region}\"]}}&limit=48";

            using var client = new HttpClient();
            
            var result = await client.GetFromJsonAsync<EnergiData>(url, cancellationToken);

            var spotPrices = result?.Records?.Select(x => new SpotPrice
                {
                    Hour = new DateTimeOffset(DateTime.Parse(x.HourUtc ?? throw new InvalidDataException(nameof(x.HourUtc))), TimeSpan.Zero)
                        .ToOffset(now.Offset),
                    Region = (x.PriceArea ?? _region).ToLowerInvariant(),
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