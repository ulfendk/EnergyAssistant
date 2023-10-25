using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UlfenDk.EnergyAssistant.Database;

// ReSharper disable once UnusedType.Global
/// <summary>
/// Enables EF Core migrations, as this is in a library project.
/// </summary>
public class EnergyAssistantContextFactory : IDesignTimeDbContextFactory<EnergyAssistantContext>
{
    public EnergyAssistantContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnergyAssistantContext>();
        optionsBuilder.UseSqlite("Data Source=design.db");

        return new EnergyAssistantContext(optionsBuilder.Options);
    }
}