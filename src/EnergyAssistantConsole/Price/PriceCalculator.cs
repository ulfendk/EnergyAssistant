using System.Collections.Immutable;
using UlfenDk.EnergyAssistant.Model;

namespace UlfenDk.EnergyAssistant.Price;

public class PriceCalculator
{
    private readonly decimal _vat;
    private readonly TariffCollection _tariffs;
    private readonly IImmutableList<(decimal Value, string Name)> _levels;

    public PriceCalculator(TariffCollection tariffs, decimal? vat, IDictionary<string, float> levels)
    {
        _vat = 1m + (vat ?? 0m);
        _tariffs = tariffs ?? throw new ArgumentNullException(nameof(tariffs));
        _levels = levels?.OrderByDescending(kv => kv.Value).Select(kv => ((decimal)kv.Value, kv.Key)).ToImmutableList() ?? throw new ArgumentNullException(nameof(levels));
    }

    public SpotPrice AddCosts(SpotPrice price)
    {
        var tariff = _tariffs[price.Hour];

        var regularPrice = _vat * (price.RawPrice + tariff.RegularCost + tariff.AdditionalCost);
        var reducedPrice = _vat * (price.RawPrice + tariff.ReducedCost + tariff.AdditionalCost);
        
        var regularLevel = _levels.Where(x => x.Value < regularPrice).ToArray() switch
        {
            [(_, { } name), _] => name,
            [(_, { } name)] => name,
            _ => _levels.Any() ? _levels.First().Name : "N/A"
        };
        var reducedLevel = _levels.Where(x => x.Value < reducedPrice).ToArray() switch
        {
            [(_, { } name), _] => name,
            [(_, { } name)] => name,
            _ => _levels.Any() ? _levels.First().Name : "N/A"
        };
        
        return price with
        {
            CalculatedPriceRegularTariff = regularPrice * _vat,
            CalculatedPriceReducedTariff = reducedPrice * _vat,
            
            RegularPriceLevel = regularLevel,
            ReducedPriceLevel = reducedLevel
        };
    }
}