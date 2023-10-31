using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public class PriceOptions : IOptionsWithDefaults<PriceOptions>
{
    public decimal? Vat { get; set; }

    [YamlMember(Alias = "tariffs")]
    public TariffPeriod[]? TariffPeriods { get; set; }

    public Dictionary<string, float>? Levels { get; set; }

    public PriceOptions Default => new PriceOptions
    {
        Vat = 0.25m,
        TariffPeriods = new []
        {
            new TariffPeriod
            {
                StartDate = new DateOnly(2023, 10, 1),
                EndDate = new DateOnly(2023, 12, 31),
                Daily = new []
                {
                    new TariffOption
                    {
                        StartTime = new TimeOnly(0, 0),
                        EndTime = new TimeOnly(6, 0),
                        AlwaysInclude = new []
                        {
                            0.1215m,
                            0.016m,
                            0.014m
                        },
                        Reduced = new []
                        {
                            0.008m
                        },
                        Standard = new []
                        {
                            0.6970m
                        }
                    },                    

                    new TariffOption
                    {
                        StartTime = new TimeOnly(6, 0),
                        EndTime = new TimeOnly(17, 0),
                        AlwaysInclude = new []
                        {
                            0.3645m,
                            0.016m,
                            0.014m
                        },
                        Reduced = new []
                        {
                            0.008m
                        },
                        Standard = new []
                        {
                            0.6970m
                        }
                    },                    

                    new TariffOption
                    {
                        StartTime = new TimeOnly(17, 0),
                        EndTime = new TimeOnly(21, 0),
                        AlwaysInclude = new []
                        {
                            1.0934m,
                            0.016m,
                            0.014m
                        },
                        Reduced = new []
                        {
                            0.008m
                        },
                        Standard = new []
                        {
                            0.6970m
                        }
                    },

                    new TariffOption
                    {
                        StartTime = new TimeOnly(21, 0),
                        EndTime = new TimeOnly(0, 0),
                        AlwaysInclude = new []
                        {
                            0.3645m,
                            0.016m,
                            0.014m
                        },
                        Reduced = new []
                        {
                            0.008m
                        },
                        Standard = new []
                        {
                            0.6970m
                        }
                    },                    
                }
            }
        },
        Levels = new Dictionary<string, float>
        {
            ["Free"] = -1000f,
            ["Low"] = 0f,
            ["Medium"] = 1.5f,
            ["High"] = 2.5f,
            ["Extreme"] = 4f
        }

    };
}