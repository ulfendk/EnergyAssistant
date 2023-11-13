using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

[AttributeUsage(AttributeTargets.Field, AllowMultiple=false, Inherited=false)]
public class UnitOfMeasurementAttribute : Attribute
{
    public string UnitOfMeasurement { get; }

    public UnitOfMeasurementAttribute(string unitOfMeasurement)
    {
        UnitOfMeasurement = unitOfMeasurement;
    }
}

public enum NewFeeApplication
{
    [UnitOfMeasurement("DKK")]
    Yearly,
    
    [UnitOfMeasurement("DKK")]
    Monthly,
    
    [UnitOfMeasurement("DKK / kWh")]
    AlwaysPerUnit,
    
    [UnitOfMeasurement("DKK / kWh")]
    RegularFeePerUnit,

    [UnitOfMeasurement("DKK / kWh")]
    ReducedFeePerUnit
}

public class NewFee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public NewFeeApplication Application { get; set; }

    public string Name { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public TimeOnly? DailyIntervalStart { get; set; }

    public decimal Amount { get; set; }

    [NotMapped]
    public bool IsValid => Application switch
    {
        NewFeeApplication.Yearly => IsPeriodValid && DailyIntervalStart is null,
        NewFeeApplication.Monthly => IsPeriodValid && DailyIntervalStart is null,
        NewFeeApplication.AlwaysPerUnit => IsPeriodValid,
        NewFeeApplication.RegularFeePerUnit => IsPeriodValid,
        NewFeeApplication.ReducedFeePerUnit => IsPeriodValid,
        _ => false
    };
    
    [NotMapped]
    public string UnitOfMeasurement =>
        typeof(NewFeeApplication).GetCustomAttribute(typeof(UnitOfMeasurementAttribute)) switch
        {
            UnitOfMeasurementAttribute uom => uom.UnitOfMeasurement,
            _ => string.Empty
        };
    
    private bool IsPeriodValid => PeriodStart < PeriodEnd;
}


public class YearPeriod
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public int Year { get; set; } = DateTimeOffset.Now.Year;

    public decimal Vat { get; set; } = 0.25m;

    public List<Fee> YearlyCosts { get; set; } = new();
}

public class Fee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; }
    
    public decimal Amount { get; set; }
}

public class MonthPeriod
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public DateOnly From { get; set; }

    public DateOnly To { get; set; }

    public List<Fee> MonthlyCosts { get; set; } = new();

    public List<Fee> RegularFeesPerUnit { get; set; } = new();
    
    public List<Fee> ReducedFeesPerUnit { get; set; } = new();

    public List<DailyInterval> Daily { get; set; } = new();
}

public class DailyInterval
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public TimeOnly Start { get; set; }

    public List<Fee> UnitFees { get; set; } = new();
}

public class EnergyAssistantContext : DbContext
{
    public DbSet<GeneralSettings> GeneralSettings { get; set; }
    public DbSet<CarnotSettings> CarnotSettings { get; set; }
    public DbSet<ElOverblikSettings> ElOverblikSettings { get; set; }
    public DbSet<EnergiDataServiceSettings> EnergiDataServiceSettings { get; set; }
    public DbSet<HomeAssistantSettings> HomeAssistantSettings { get; set; }
    public DbSet<NordPoolSettings> NordPoolSettings { get; set; }
    
    public DbSet<SpotPriceRecord> Prices { get; set; }
    public DbSet<EnergyUsageRecord> Usages { get; set; }

    // public DbSet<FeePeriod> Periods { get; set; }
    // public DbSet<HourlyFeePeriod> HourlyFees { get; set; }
    // public DbSet<FixedFee> FixedFees { get; set; }
    // public DbSet<FeePerUnit> FeesPerUnit { get; set; }

    public DbSet<YearPeriod> YearPeriods { get; set; }
    public DbSet<MonthPeriod> MonthPeriods { get; set; }
    public DbSet<DailyInterval> DailyIntervals { get; set; }
    public DbSet<Fee> Fees { get; set; }

    public EnergyAssistantContext(DbContextOptions<EnergyAssistantContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GeneralSettings>().HasData(new GeneralSettings
        {
            Id = 1,            
            Region = "dk2",
            HasElectricHeating = false
        });

        modelBuilder.Entity<CarnotSettings>().HasData(new CarnotSettings
        {
            Id = 1,
            IsEnabled = false,
            User = "",
            ApiKey = ""
        });

        modelBuilder.Entity<ElOverblikSettings>().HasData(new ElOverblikSettings
        {
            Id = 1,
            IsEnabled = false,
            ApiToken = "",
            MeteringPointId = "",
            ElectricHeatingMeteringPointId = ""
        });

        modelBuilder.Entity<EnergiDataServiceSettings>().HasData(new EnergiDataServiceSettings
        {
            Id = 1,
            IsEnabled = true
        });

        modelBuilder.Entity<HomeAssistantSettings>().HasData(new HomeAssistantSettings
        {
            Id = 1,
            IsEnabled = false,
            Url = "http://supervisor/core",
            Token = ""
        });

        modelBuilder.Entity<NordPoolSettings>().HasData(new NordPoolSettings
        {
            Id = 1,
            IsEnabled = false
        });
        
        base.OnModelCreating(modelBuilder);
    }
}