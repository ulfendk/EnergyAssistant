using System.Globalization;
using System.Net.Http.Json;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.Nordpool;

public class NordpoolDataLoader
{
    private readonly string _region;

    public NordpoolDataLoader(string region)
    {
        _region = region ?? throw new ArgumentNullException(nameof(region));
    }

    public async Task<SpotPrice[]> GetSpotPricesAsync()
    {
        try
        {
            var now = DateTimeOffset.Now;
            
            Console.Write($"Downloading prices from Nordpool...");
            
            string uri = $"https://www.nordpoolgroup.com/api/marketdata/page/41?currency=,DKK,DKK,EUR&endDate={now.ToString("dd-MM-yyyy")}";
            using var client = new HttpClient();

            var result = await client.GetFromJsonAsync<Welcome>(uri, Converter.Settings);

            if (result is null)
            {
                return Array.Empty<SpotPrice>();
            }

            var spotPrices = result.Data.Rows.Take(24).Select((x, i) =>
            {
                var column = x.Columns.SingleOrDefault(y => y.Name?.ToString().Equals(_region, StringComparison.OrdinalIgnoreCase) ?? false)
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
            });
            
            Console.WriteLine("Done");

            return spotPrices.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download from Nordpool: {ex.Message}");

            return Array.Empty<SpotPrice>();
        }
    }
}