using System.Net.Http.Json;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergiDataService.Carnot;

public class CarnotDataLoader
{
    private readonly string _user;

    private readonly string _apiKey;

    private readonly string _region;

    public CarnotDataLoader(string user, string apiKey, string region)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _region = region ?? throw new ArgumentNullException(nameof(region));
    }

    public async Task<SpotPrice[]> GetPredictionsAsync()
    {
        try
        {
            Console.Write($"Downloading predictions from Carnot.dk...");
            string uri = $"https://whale-app-dquqw.ondigitalocean.app/openapi/get_predict?energysource=spotprice&region={_region}&daysahead=7";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", _apiKey);
            client.DefaultRequestHeaders.Add("username", _user);

            var result = await client.GetFromJsonAsync<CarnotData>(uri);

            var spotPrices = result?.Predictions?.Select(x => new SpotPrice
            {
                    Hour = DateTimeOffset.Parse(x.Utctime ?? throw new InvalidDataException(nameof(x.Utctime))),
                    Region = (x.Pricearea ?? _region).ToLowerInvariant(),
                    Source = "Carnot.dk",
                    RawPrice = (decimal?)x.Prediction ?? throw new InvalidDataException(nameof(x.Prediction)),
                    IsPrediction = true
            });
            
            Console.WriteLine("Done");

            return spotPrices?.ToArray() ?? Array.Empty<SpotPrice>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download from Carnot.dk: {ex.Message}");

            return Array.Empty<SpotPrice>();
        }
    }
}