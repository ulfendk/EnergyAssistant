using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Database;
using UlfenDk.EnergyAssistant.Eloverblik;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.HomeAssistant;
using UlfenDk.EnergyAssistant.Nordpool;
using UlfenDk.EnergyAssistant.Price;
using UlfenDk.EnergyAssistant.Repository;

namespace UlfenDk.EnergyAssistant;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnergyAssistant(this IServiceCollection services, string dataDir)
    {
        services
            // .AddTransient<CarnotDataLoader>()
            // .AddTransient<ElOverblikLoader>()
            // .AddTransient<EnergiDataServiceLoader>()
            // .AddTransient<HomeAssistantApiClient>()
            // .AddTransient<NordpoolDataLoader>()
            // .AddTransient<PriceCalculator>()
            // .AddTransient<EnergyAssistantRepository>(s => new EnergyAssistantRepository(Path.Combine(dataDir, "data.db")));
            .AddSingleton<EnergyAssistantService>()

            .AddDbContextFactory<EnergyAssistantContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataDir, "data.db")}"))
        
            .Configure<OptionsFileOptions<GeneralOptions>>(o => o.FileName = Path.Join(dataDir, "config.yaml"))
            .Configure<OptionsFileOptions<PriceOptions>>(o => o.FileName = Path.Join(dataDir, "prices.yaml"))
            .Configure<OptionsFileOptions<HomeAssistantOptions>>(o => o.FileName = Path.Join(dataDir, "homeassistant.yaml"))
            .Configure<OptionsFileOptions<CarnotOptions>>(o => o.FileName = Path.Join(dataDir, "carnot.yaml"))
            .Configure<OptionsFileOptions<ActivityOptions>>(o => o.FileName = Path.Join(dataDir, "activities.yaml"))
            ;
            // .AddDbContext<EnergyAssistantContext>();
        
        return services;
    }
}