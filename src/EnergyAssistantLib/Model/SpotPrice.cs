namespace UlfenDk.EnergyAssistant.Model;

public class SpotPrice : IComparable<SpotPrice> 
{
    // public int CompareTo(SpotPrice other) => Hour.CompareTo(other.Hour);

    public override string ToString() => $"{{{Hour}, DKK {RawPrice}, {Source}}}";

    public override int GetHashCode() => (int)Hour.ToUnixTimeSeconds();
//     {
//         return $"{Region}:{Hour}".GetHashCode();
//     }
    // public virtual bool Equals(SpotPrice? other) => other switch
    // {
    //     { } price when true => Equals(price),
    //     _ => false
    // };

    public int CompareTo(SpotPrice? other) => other switch
    {
        { } price => Hour.CompareTo(price.Hour),
        _ => -1
    };

    public override bool Equals(object? obj) => obj switch
    {
        SpotPrice price => Hour == price.Hour,
        _ => false
    };

    // public override bool Equals(object? obj) => obj switch
    // {
    //     SpotPrice price => Hour == price.Hour,
    //     _ => false
    // };
    // public bool Equals(SpotPrice other) => Hour == other.Hour;
    //
    // public bool Equals(SpotPrice? other) => other switch
    // {
    //     { } price when true => Equals(price),
    //     _ => false
    // };
    // {
    //     return Region == other Hour == other?.Hour;
    // }

    public string Region { get; init; }
    
    public DateTimeOffset Hour { get; init; }
    
    public decimal RawPrice { get; init; }

    public string Source { get; init; }

    public bool IsPrediction { get; init; }
    
    public decimal CalculatedPriceRegularTariff { get; set; }

    public decimal CalculatedPriceReducedTariff { get; set;}
    
    public string RegularPriceLevel { get;  set;}

    public string ReducedPriceLevel { get;  set;}
}