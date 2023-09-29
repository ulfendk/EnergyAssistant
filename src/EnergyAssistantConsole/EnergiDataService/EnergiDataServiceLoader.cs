using System.Net.Http.Json;

namespace UlfenDk.EnergyAssistant.EnergiDataService;

public class EnergiDataServiceLoader
{
    private readonly string _region;

    public EnergiDataServiceLoader(string region)
    {
        _region = region ?? throw new ArgumentNullException(nameof(region));
    }

    public async Task<EnergiData?> GetLatestPricesAsync()
    {
        try
        {
            Console.Write($"Downloading predictions from Carnot.dk...");

            string url = $"https://api.energidataservice.dk/dataset/elspotprices?start={DateTime.Now:yyyy-MM-ddThh:mm:ss}}&sort=HourUTC asc&filter={{\"PriceArea\":[\"{_region}}\"]}}&limit=48";

            using var client = new HttpClient();
            
            var result = await client.GetFromJsonAsync<EnergiData>(url);

            Console.WriteLine("Done");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download from EnergiDataService: {ex.Message}");

            return null;
        }
    }

}