using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Mqtt;

#region Options loading

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
            new Tariff((decimal)(tariff.FixedCost ?? throw new InvalidDataException(nameof(tariff.FixedCost)))));
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

var energidataserviceLoader = new EnergiDataServiceLoader(options?.Carnot?.Region ?? throw new InvalidDataException(nameof(options.Carnot.Region)));

#endregion

#region Carnot.dk

var carnotLoader = new CarnotDataLoader(
    options?.Carnot?.User ?? throw new InvalidDataException(nameof(options.Carnot.User)),
    options?.Carnot?.ApiKey ?? throw new InvalidDataException(nameof(options.Carnot.ApiKey)),
    options?.Carnot?.Region ?? throw new InvalidDataException(nameof(options.Carnot.Region)));

#endregion

MqttPublisher GetMqttPublisher() => new MqttPublisher(options?.Mqtt ?? throw new InvalidDataException(nameof(options.Mqtt));

