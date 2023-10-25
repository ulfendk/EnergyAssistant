using System.Net;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.HomeAssistant;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Nordpool;
using UlfenDk.EnergyAssistant.Price;

#region Options loading

var dataDir = Environment.GetCommandLineArgs()[1];
if (!Directory.Exists(dataDir)) throw new ArgumentException("data directory");

var configDir = Path.Combine(dataDir, "energyassistant");
if (!Directory.Exists(configDir)) Directory.CreateDirectory(configDir);

var configFileName = Path.Combine(configDir, "config.yaml");
if (!File.Exists(configFileName))
{
    var opt = new Options
    {
        Region = "dk2",
        HomeAssistant = new HomeAssistant
        {
            Url = "http://supervisor/core"
        },
        Carnot = new CarnotOptions
        {
            IsEnabled = true,
            User = "",
            ApiKey = ""
        },
        UseNordPoolBackup = true,
        Levels = new Dictionary<string, float>
        {
            ["Free"] = -1000f,
            ["Low"] = 0f,
            ["Medium"] = 1.5f,
            ["High"] = 2.5f,
            ["Extreme"] = 4f
        },
        Vat = 0.25f,
        TariffPeriods = new[]
        {
            new TariffPeriod
            {
                StartDate = new DateOnly(2023, 10, 1),
                EndDate = new DateOnly(2023, 12, 31),
                Daily = new[]
                {
                    new TariffOption
                    {
                        StartTime = new TimeOnly(0, 0),
                        EndTime = new TimeOnly(6, 0),
                        AlwaysInclude = new[]
                        {
                            0.1215m
                        },
                        Standard = new[]
                        {
                            0.6970m
                        },
                        Reduced = new[]
                        {
                            0.0080m
                        }
                    },
                    new TariffOption
                    {
                        StartTime = new TimeOnly(6, 0),
                        EndTime = new TimeOnly(17, 0),
                        AlwaysInclude = new[]
                        {
                            0.3645m
                        },
                        Standard = new[]
                        {
                            0.6970m
                        },
                        Reduced = new[]
                        {
                            0.008m
                        }
                    },
                    new TariffOption
                    {
                        StartTime = new TimeOnly(17, 0),
                        EndTime = new TimeOnly(21, 0),
                        AlwaysInclude = new[]
                        {
                            1.0934m
                        },
                        Standard = new[]
                        {
                            0.6970m
                        },
                        Reduced = new[]
                        {
                            0.008m
                        }
                    },
                    new TariffOption
                    {
                        StartTime = new TimeOnly(21, 0),
                        EndTime = new TimeOnly(0, 0),
                        AlwaysInclude = new[]
                        {
                            0.3645m
                        },
                        Standard = new[]
                        {
                            0.6970m
                        },
                        Reduced = new[]
                        {
                            0.008m
                        }
                    }
                }
            }
        },
        Spans = new[]
        {
            new ActivitySpan
            {
                Title = "Tøjvask",
                HoursOfDay = new[] { 1, 2, 3, 4, 6, 7, 8 },
                HoursDuration = 3,
                MaxHoursInFuture = 12
            }
        }
    };

    OptionsLoader.WriteOptions(configFileName, opt);
}

var options = OptionsLoader.GetOptions(configFileName);

var tariffs = new TariffCollection(options.TariffPeriods);

#endregion

#region Energi Data Service

var energiDataServiceLoader = new EnergiDataServiceLoader(options.Region ?? throw new InvalidDataException(nameof(options.Region)));

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

var haClient = new HomeAssistantApiClient(options?.HomeAssistant, options.Region);

var spotPrices = new SpotPriceCollection();

var lastUpdated = DateTimeOffset.Now;
var lastChanged = DateTimeOffset.Now;
var nextDataRefresh = DateTimeOffset.MinValue;
while (true)
{
    Console.WriteLine($"[{DateTimeOffset.Now:s}]");

    if (DateTimeOffset.Now >= nextDataRefresh)
    {
        if (options.Carnot.IsEnabled)
        {
            var carnotData = await carnotLoader.GetPredictionsAsync();
            if (carnotData.Any())
            {
                spotPrices.AddOrUpdateRange(carnotData.Select(priceCalculator.AddCosts));
            }
        }

        var energiData = await energiDataServiceLoader.GetLatestPricesAsync();
        if (energiData.Any())
        {
            spotPrices.AddOrUpdateRange(energiData.Select(priceCalculator.AddCosts));
        }
        else if (options.UseNordPoolBackup)
        {
            var nordpoolData = await nordpoolLoader.GetSpotPricesAsync();
            spotPrices.AddOrUpdateRange(nordpoolData.Select(priceCalculator.AddCosts));
        }
        
        lastChanged = DateTimeOffset.Now;
    }

    Console.WriteLine("Updating prices in Home Assistant.");
    
    await haClient.UpdatePrice(
        "spotprice_normal",
        "Spotprice (Normal)",
        spotPrices,
        x => (x.CalculatedPriceRegularTariff, x.RegularPriceLevel),
        lastChanged);

    await haClient.UpdatePrice(
        "spotprice_reduced",
        "Spotprice (Reduced)",
        spotPrices,
        x => (x.CalculatedPriceReducedTariff, x.ReducedPriceLevel),
        lastChanged);

    await haClient.UpdatePrice(
        "spotprice_raw",
        "Spotprice (Raw)",
        spotPrices,
        x => (x.RawPrice, null),
        lastChanged,
        includeLevel: false);
    
    lastUpdated = DateTimeOffset.Now;
    
    nextDataRefresh = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(29 - DateTime.Now.Minute % 30).Add(TimeSpan.FromSeconds(60 - DateTime.Now.Second)));
    var sleepTime = TimeSpan.FromMinutes(4 - DateTime.Now.Minute % 5).Add(TimeSpan.FromSeconds(61-DateTime.Now.Second));
    Console.WriteLine($"Next data refresh is at {nextDataRefresh:s} and sensor data in Home Assistant will be refreshed every 5 minutes.");

    await Task.Delay(sleepTime);
    Console.WriteLine();
}

