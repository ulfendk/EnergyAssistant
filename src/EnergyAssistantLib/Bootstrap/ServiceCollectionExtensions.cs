using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UlfenDk.EnergiDataService.Carnot;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Database;
using UlfenDk.EnergyAssistant.Eloverblik;
using UlfenDk.EnergyAssistant.EnergiDataService;
using UlfenDk.EnergyAssistant.HomeAssistant;
using UlfenDk.EnergyAssistant.Model;
using UlfenDk.EnergyAssistant.Nordpool;
using UlfenDk.EnergyAssistant.Price;
using UlfenDk.EnergyAssistant.Repository;

namespace UlfenDk.EnergyAssistant;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnergyAssistant(this IServiceCollection services, string dataDir)
    {
        services
            // .Configure<OptionsFileOptions<GeneralOptions>>(o => o.FileName = Path.Join(dataDir, "general.yaml"))
            .Configure<OptionsFileOptions<PriceOptions>>(o => o.FileName = Path.Join(dataDir, "prices.yaml"))
            // .Configure<OptionsFileOptions<HomeAssistantOptions>>(o => o.FileName = Path.Join(dataDir, "homeassistant.yaml"))
            // .Configure<OptionsFileOptions<CarnotOptions>>(o => o.FileName = Path.Join(dataDir, "carnot.yaml"))
            // .Configure<OptionsFileOptions<EnergiDataServiceOptions>>(o => o.FileName = Path.Join(dataDir, "energidataservice.yaml"))
            // .Configure<OptionsFileOptions<NordpoolOptions>>(o => o.FileName = Path.Join(dataDir, "nordpool.yaml"))
            // .Configure<OptionsFileOptions<EloverblikOptions>>(o => o.FileName = Path.Join(dataDir, "eloverblik.yaml"))
            .Configure<OptionsFileOptions<ActivityOptions>>(o => o.FileName = Path.Join(dataDir, "activities.yaml"))

            .AddTransient(typeof(OptionsLoader<>))
            
            .AddSingleton<SpotPriceCollection>()
            .AddSingleton<CarnotDataLoader>()
            .AddSingleton<ElOverblikLoader>()
            .AddSingleton<EnergiDataServiceLoader>()
            .AddSingleton<HomeAssistantApiClient>()
            .AddSingleton<NordpoolDataLoader>()
            .AddSingleton<PriceCalculator>()
            // .AddTransient<EnergyAssistantRepository>(s => new EnergyAssistantRepository(Path.Combine(dataDir, "data.db")));
            .AddSingleton<EnergyAssistantService>()

            .AddDbContextFactory<EnergyAssistantContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(dataDir, "data.db")}"))
            
            .AddTransient<EnergyAssistantRepository>()
        
            ;
            // .AddDbContext<EnergyAssistantContext>();
        
        return services;
    }
}