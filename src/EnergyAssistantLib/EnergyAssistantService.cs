using Microsoft.Extensions.Logging;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Eloverblik;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.HomeAssistant;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Nordpool;
using UlfenDk.EnergyAssistant.Price;

namespace UlfenDk.EnergyAssistant;

public class EnergyAssistantService
{
    private readonly CarnotDataLoader _carnotDataLoader;
    private readonly ElOverblikLoader _elOverblikLoader;
    private readonly EnergiDataServiceLoader _energiDataServiceLoader;
    private readonly HomeAssistantApiClient _homeAssistantApiClient;
    private readonly NordpoolDataLoader _nordpoolDataLoader;
    private readonly PriceCalculator _priceCalculator;
    private readonly SpotPriceCollection _spotPrices;
    private readonly ILogger<EnergyAssistantService> _logger;

    private readonly CancellationTokenSource _cts;

    private Task? _task;

    public EnergyAssistantService(
        CarnotDataLoader carnotDataLoader,
        ElOverblikLoader elOverblikLoader,
        EnergiDataServiceLoader energiDataServiceLoader,
        HomeAssistantApiClient homeAssistantApiClient,
        NordpoolDataLoader nordpoolDataLoader,
        PriceCalculator priceCalculator,
        SpotPriceCollection spotPrices,
        ILogger<EnergyAssistantService> logger)
    {
        _carnotDataLoader = carnotDataLoader ?? throw new ArgumentNullException(nameof(carnotDataLoader));
        _elOverblikLoader = elOverblikLoader ?? throw new ArgumentNullException(nameof(elOverblikLoader));
        _energiDataServiceLoader = energiDataServiceLoader ?? throw new ArgumentNullException(nameof(energiDataServiceLoader));
        _homeAssistantApiClient = homeAssistantApiClient ?? throw new ArgumentNullException(nameof(homeAssistantApiClient));
        _nordpoolDataLoader = nordpoolDataLoader ?? throw new ArgumentNullException(nameof(nordpoolDataLoader));
        _priceCalculator = priceCalculator ?? throw new ArgumentNullException(nameof(priceCalculator));
        _spotPrices = spotPrices ?? throw new ArgumentNullException(nameof(spotPrices));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _cts = new CancellationTokenSource();
    }

    public void Start()
    {
        _logger.LogInformation("Starting service.");

        _task = RunAsync(_cts.Token);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Stopping service.");

            if (_task?.Status == TaskStatus.Running || _task?.Status == TaskStatus.WaitingForActivation)
            {
                var timeout = TimeSpan.FromSeconds(10);
                
                _cts.Cancel();
                
                _logger.LogInformation("Waiting {timeout:ss} seconds for service to stop.", timeout);
                try
                {
                    await _task.WaitAsync(timeout, cancellationToken);
                }
                catch (TimeoutException)
                {
                    _logger.LogError("Service cannot be stopped: Service did not stop within the given timeout.");
                }
            }
            else
            {
                _logger.LogError("Service cannot be stopped: Service is not running.");
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Service is stopped.");
        }
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
         var lastUpdated = DateTimeOffset.Now;
         var lastChanged = DateTimeOffset.Now;
         var nextDataRefresh = DateTimeOffset.MinValue;
         while (!cancellationToken.IsCancellationRequested)
         {
             var now = DateTimeOffset.Now;
             if (DateTimeOffset.Now >= nextDataRefresh)
             {
                 if (_carnotDataLoader.IsEnabled)
                 {
                     var carnotData = await _carnotDataLoader.GetPredictionsAsync(cancellationToken);
                     if (carnotData.Any())
                     {
                         _spotPrices.AddOrUpdateRange(carnotData.Select(_priceCalculator.AddCosts));
                     }
                 }

                 var energiData = await _energiDataServiceLoader.GetLatestPricesAsync(cancellationToken);
                 if (energiData.Any())
                 {
                     _spotPrices.AddOrUpdateRange(energiData.Select(_priceCalculator.AddCosts));
                 }
                 else if (_nordpoolDataLoader.IsEnabled)
                 {
                     var nordpoolData = await _nordpoolDataLoader.GetSpotPricesAsync(cancellationToken);
                     _spotPrices.AddOrUpdateRange(nordpoolData.Select(_priceCalculator.AddCosts));
                 }
         
                 lastChanged = DateTimeOffset.Now;
             }

             await UpdateHomeAssistantEntitiesAsync(lastChanged, cancellationToken);
             
             lastUpdated = DateTimeOffset.Now;
     
             nextDataRefresh = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(29 - DateTime.Now.Minute % 30).Add(TimeSpan.FromSeconds(60 - DateTime.Now.Second)));
             var sleepTime = TimeSpan.FromMinutes(4 - DateTime.Now.Minute % 5).Add(TimeSpan.FromSeconds(61-DateTime.Now.Second));
             
             _logger.LogInformation("Next data refresh is at {nextDataRefresh:s} and sensor data in Home Assistant will be refreshed every 5 minutes.", nextDataRefresh);

             await Task.Delay(sleepTime, cancellationToken);
         }
    }

    private async Task UpdateHomeAssistantEntitiesAsync(DateTimeOffset lastChanged, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating entities in Home Assistant.");
     
        await _homeAssistantApiClient.UpdatePriceAsync(
            "spotprice_normal",
            "Spotprice (Normal)",
            _spotPrices,
            x => (x.CalculatedPriceRegularTariff, x.RegularPriceLevel),
            lastChanged,
            true,
            cancellationToken);

        await _homeAssistantApiClient.UpdatePriceAsync(
            "spotprice_reduced",
            "Spotprice (Reduced)",
            _spotPrices,
            x => (x.CalculatedPriceReducedTariff, x.ReducedPriceLevel),
            lastChanged,
            true,
            cancellationToken);

        await _homeAssistantApiClient.UpdatePriceAsync(
            "spotprice_raw",
            "Spotprice (Raw)",
            _spotPrices,
            x => (x.RawPrice, null),
            lastChanged,
            false,
            cancellationToken);
    }
}