using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

public class MonthlyFixedFee
{
    public long Id { get; set; }
    
    public required string Name { get; set; }

    public decimal Amount { get; set; }
}

public enum VariableFeeApplication
{
    Always,
    BelowAndIncludingLimit,
    BelowLimit,
    AboveAndIncludingLimit,
    AboveLimit
}

public class DailyVariableFee
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public decimal AmountPerUnit { get; set; }

    public VariableFeeApplication Application { get; set; }

    public decimal LimitInUnits { get; set; }
}

public class FeePerUnit
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public decimal AmountPerUnit { get; set; }
}

public class EnergyAssistantContext : DbContext
{
    public DbSet<SpotPriceRecord> Prices { get; set; }
    public DbSet<EnergyUsageRecord> Usages { get; set; }

    public EnergyAssistantContext(DbContextOptions<EnergyAssistantContext> options)
        : base(options)
    { }
}