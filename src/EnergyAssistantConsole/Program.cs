using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.HomeAssistant;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Mqtt;
using UlfenDk.EnergyAssistant.Nordpool;
using UlfenDk.EnergyAssistant.Price;

#region Options loading

// var opt = new Options
// {
//     Region = "dk2",
//     Carnot = new CarnotOptions
//     {
//         User = "regin@ulfen.dk",
//         ApiKey = ""
//     },
//     Mqtt = new MqttOptions
//     {
//         Server = "192.168.42.170",
//         Port = 1883,
//         ClientId = "energyassistant",
//         User = "energyassistant",
//         Password = "",
//         UseTls = false,
//         AllowUntrustedCertificate = true
//     },
//     Levels = new Dictionary<string, float>
//     {
//         ["Gratis"] = -1000f,
//         ["Lav"] = 0f,
//         ["Mellem"] = 1.5f,
//         ["Høj"] = 2.5f,
//         ["Ekstrem"] = 4f
//     },
//     Vat = 0.25f,
//     TariffPeriods = new []
//     {
//         new TariffPeriod
//         {
//             Name = "2023Q4",
//             Start = new DateOnly(2023,10, 1),
//             End = new DateOnly(2023, 12, 31)
//         }
//     },
//     Tariffs = new []
//     {
//         new TariffOption
//         {
//             Period = "2023Q4",
//             StartTime = new TimeOnly(0,0),
//             EndTime = new TimeOnly(6,0),
//             CommonCosts = new []
//             {
//                 0.02f,
//                 0.24f,
//             },
//             RegularAdditionalCosts = new []
//             {
//                 1.13f
//             },
//             ReducedAdditionalCosts = new []
//             {
//                 0.27f
//             }
//         }
//     },
//     Spans = new []
//     {
//         new ActivitySpan
//         {
//             Title = "Tøjvask",
//             HoursOfDay = new [] { 1, 2, 3, 4, 6, 7, 8 },
//             HoursDuration = 3,
//             MaxHoursInFuture = 12
//         }
//     }
// };
//
// var serialized = JsonSerializer.Serialize(opt, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
// File.WriteAllText("/home/regin/energyassistant.json", serialized);

var options = OptionsLoader.GetOptions(Environment.GetCommandLineArgs()[1]);

var tariffPeriods = options?.TariffPeriods ?? throw new InvalidDataException(nameof(options.TariffPeriods)); 
var tariffsInPeriods = options?.Tariffs ?? throw new InvalidDataException(nameof(options.Tariffs));
var tariffsDictionary = new Dictionary<CalendarInterval, IDictionary<TimeInterval, Tariff>>();
foreach (var period in tariffPeriods)
{
    var tariffsForPeriod = new Dictionary<TimeInterval, Tariff>();
    foreach (var tariff in  tariffsInPeriods.Where(x => x.Period == period.Name))
    {
        tariffsForPeriod.Add(
            new TimeInterval(
                tariff.StartTime ?? throw new InvalidDataException(nameof(tariff.StartTime)),
                tariff.EndTime ?? throw new InvalidDataException(nameof(tariff.EndTime))),
            new Tariff(
                (decimal)tariff.RegularFixedCost, 
                (decimal)tariff.ReducedFixedCost));
    }

    tariffsDictionary.Add(
        new CalendarInterval(
            period?.Start ?? throw new InvalidDataException(nameof(period.Start)),
            period?.End ?? throw new InvalidDataException(nameof(period.End))),
        tariffsForPeriod);
}

var tariffs = new TariffCollection(tariffsDictionary);

#endregion

#region Energi Data Service

var energidataserviceLoader = new EnergiDataServiceLoader(options.Region ?? throw new InvalidDataException(nameof(options.Region)));

#endregion

#region Carnot.dk

var carnotLoader = new CarnotDataLoader(
    options.Carnot?.User ?? throw new InvalidDataException(nameof(options.Carnot.User)),
    options.Carnot?.ApiKey ?? throw new InvalidDataException(nameof(options.Carnot.ApiKey)),
    options.Region ?? throw new InvalidDataException(nameof(options.Region)));

#endregion

#region Nordpool

var nordpoolLoader = new NordpoolDataLoader(options.Region);

#endregion

var priceCalculator = new PriceCalculator(tariffs, (decimal)options.Vat, options.Levels);

MqttPublisher GetMqttPublisher() => new MqttPublisher(options?.Mqtt ?? throw new InvalidDataException(nameof(options.Mqtt)));

using (var mqttDiscoveryPublisher = GetMqttPublisher())
{
    // Publish Discovery messages here
}


var spotPrices = new SpotPriceCollection();

while (true)
{
    var now = DateTimeOffset.Now;

    Console.WriteLine($"[{now:g}]");
    var carnotData = await carnotLoader.GetPredictionsAsync();
    if (carnotData.Any())
    {
        spotPrices.AddOrUpdateRange(carnotData.Select(priceCalculator.AddCosts));
    }

    var energiData = await energidataserviceLoader.GetLatestPricesAsync();
    if (energiData.Any())
    {
        spotPrices.AddOrUpdateRange(energiData.Select(priceCalculator.AddCosts));
    }
    else if (options.UseNordPoolBackup)
    {
        var nordpoolData = await nordpoolLoader.GetSpotPricesAsync();
        spotPrices.AddOrUpdateRange(nordpoolData.Select(priceCalculator.AddCosts));
    }

    // Publish values
    using var mqtt = GetMqttPublisher();
    var regularSpotPriceSensor = new ListState(spotPrices.Current.CalculatedPriceRegularTariff,
        spotPrices.Current.RegularPriceLevel,
        spotPrices.Select(x =>
            new HourPrice(x.Hour, x.CalculatedPriceRegularTariff, x.RegularPriceLevel, x.IsPrediction)).ToArray(),
        now);
    var reducedPriceSensor = new ListState(spotPrices.Current.CalculatedPriceReducedTariff,
        spotPrices.Current.ReducedPriceLevel,
        spotPrices.Select(x =>
            new HourPrice(x.Hour, x.CalculatedPriceReducedTariff, x.ReducedPriceLevel, x.IsPrediction)).ToArray(),
        now);
    var rawSpotPriceSensor = new ListState(spotPrices.Current.RawPrice,
        "N/A",
        spotPrices.Select(x =>
            new HourPrice(x.Hour, x.RawPrice, "N/A", x.IsPrediction)).ToArray(),
        now);

    var sleepTime = TimeSpan.FromMinutes(30 - DateTime.Now.Minute % 30 + 1);
    Console.Write($"Sleeping for {sleepTime:mm} minutes...");
    await Task.Delay(sleepTime);
    Console.WriteLine("Done");
    Console.WriteLine();
}

