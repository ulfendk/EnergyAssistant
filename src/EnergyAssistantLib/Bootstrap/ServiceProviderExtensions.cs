using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UlfenDk.EnergyAssistant.Database;

namespace UlfenDk.EnergyAssistant;

public static class ServiceProviderExtensions
{
    public static async Task<IServiceProvider> MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var dbContext = await serviceProvider.GetRequiredService<IDbContextFactory<EnergyAssistantContext>>()
            .CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();

        return serviceProvider;
    }
}