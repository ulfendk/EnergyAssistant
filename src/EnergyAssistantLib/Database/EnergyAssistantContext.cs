using Microsoft.EntityFrameworkCore;

namespace UlfenDk.EnergyAssistant.Database;

public class EnergyAssistantContext : DbContext
{
    public DbSet<SpotPriceRecord> Prices { get; set; }
    public DbSet<EnergyUsageRecord> Usages { get; set; }

    public EnergyAssistantContext(DbContextOptions<EnergyAssistantContext> options)
        : base(options)
    { }
}