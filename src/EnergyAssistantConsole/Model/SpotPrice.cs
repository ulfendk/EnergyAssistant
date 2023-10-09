namespace UlfenDk.EnergyAssistant.Model;

public record struct SpotPrice(string Region, DateTimeOffset Hour, decimal RawPrice, string Source, bool IsPrediction)
{
    public override int GetHashCode()
    {
        return Hour.GetHashCode();
    }

    public bool Equals(SpotPrice? other)
    {
        return Hour == other?.Hour;
    }

    public decimal CalculatedPriceRegularTariff { get; set; }

    public decimal CalculatedPriceReducedTariff { get; set; }

    public decimal RawPrice { get; set; }

    public string RegularPriceLevel { get; set; }

    public string ReducedPriceLevel { get; set; }
}