using System.Net.Http.Json;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.EnergiDataService;

public class EnergiDataServiceLoader
{
    private readonly string _region;

    public EnergiDataServiceLoader(string region)
    {
        _region = region ?? throw new ArgumentNullException(nameof(region));
    }

    public async Task<SpotPrice[]> GetLatestPricesAsync()
    {
        try
        {
            Console.Write($"Downloading predictions from EnergiDataService...");

            var now = DateTimeOffset.Now;

            string url = $"https://api.energidataservice.dk/dataset/elspotprices?start={now:yyyy-MM-dd}&sort=HourUTC asc&filter={{\"PriceArea\":[\"{_region}\"]}}&limit=48";

            using var client = new HttpClient();
            
            var result = await client.GetFromJsonAsync<EnergiData>(url);
            
            var spotPrices = result?.Records?.Select(x => new SpotPrice
            {
                Hour = new DateTimeOffset(DateTime.Parse(x.HourUtc ?? throw new InvalidDataException(nameof(x.HourUtc))), now.Offset),
                Region = (x.PriceArea ?? _region).ToLowerInvariant(),
                RawPrice = (decimal?)x.SpotPriceDkk ?? throw new InvalidDataException(nameof(x.SpotPriceDkk)),
                IsPrediction = false,
                Source = "EnergiDataService"
            }).ToArray() ?? Array.Empty<SpotPrice>();
            
            Console.WriteLine("Done");

            return spotPrices;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download from EnergiDataService: {ex.Message}");

            return Array.Empty<SpotPrice>();
        }
    }

}