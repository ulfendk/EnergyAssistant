using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UlfenDk.EnergyAssistant.Database;
using UlfenDk.EnergyAssistant.Helpers;

namespace UlfenDk.EnergyAssistant.Repository;

public class EnergyAssistantRepository
{
    private readonly IDbContextFactory<EnergyAssistantContext> _dbContextFactory;
    private readonly ILogger<EnergyAssistantRepository> _logger;

    public EnergyAssistantRepository(IDbContextFactory<EnergyAssistantContext> dbContextFactory, ILogger<EnergyAssistantRepository> logger)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<GeneralSettings> GetGeneralSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.GeneralSettings.AsNoTracking().FirstAsync();
    }

    public async Task<CarnotSettings> GetCarnotSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.CarnotSettings.AsNoTracking().FirstAsync();
    }

    public async Task<ElOverblikSettings> GetElOverblikSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.ElOverblikSettings.AsNoTracking().FirstAsync();
    }
    public async Task<EnergiDataServiceSettings> GetEnergiDataServiceSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.EnergiDataServiceSettings.AsNoTracking().FirstAsync();
    }
    public async Task<HomeAssistantSettings> GetHomeAssistantSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.HomeAssistantSettings.AsNoTracking().FirstAsync();
    }
    public async Task<NordPoolSettings> GetNordPoolSettingsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        return await ctx.NordPoolSettings.AsNoTracking().FirstAsync();
    }
    
    public async Task UpdateGeneralSettingsAsync(Action<GeneralSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.GeneralSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }
    public async Task UpdateCarnotSettingsAsync(Action<CarnotSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.CarnotSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateElOverblikSettingsAsync(Action<ElOverblikSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.ElOverblikSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateHomeAssistantSettingsAsync(Action<HomeAssistantSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.HomeAssistantSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateEnergiDataServiceSettingsAsync(Action<EnergiDataServiceSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.EnergiDataServiceSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }
    
    public async Task UpdateNordPoolSettingsAsync(Action<NordPoolSettings> settingsUpdater)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var setting = await ctx.NordPoolSettings.FirstAsync();
        
        settingsUpdater(setting);
        
        await ctx.SaveChangesAsync();
    }

    public async Task<List<YearPeriod>> GetYearlyPeriodsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var period =
            from p in ctx.YearPeriods.AsNoTracking()
                .Include(x => x.YearlyCosts).AsNoTracking()
            select p;

        return await period.ToListAsync();
    }

    public async Task<List<MonthPeriod>> GetMonthPeriodsAsync()
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var periods =
            from p in ctx.MonthPeriods.AsNoTracking()
                .Include(x => x.MonthlyCosts).AsNoTracking()
                .Include(x => x.RegularFeesPerUnit).AsNoTracking()
                .Include(x => x.ReducedFeesPerUnit).AsNoTracking()
                .Include(x => x.Daily)
                    .ThenInclude(x => x.UnitFees).AsNoTracking()
            select p;

        return await periods.ToListAsync();
    }

    // public async Task<List<YearPeriod>> GetYearlyPeriodsAsync()
    // {
    //     var ctx = await _dbContextFactory.CreateDbContextAsync();
    //
    //     var periods =
    //         from p in ctx.YearPeriods.AsNoTracking()
    //             .Include(x => x.YearlyCosts).AsNoTracking()
    //             .Include(x => x.Monthly)
    //                 .ThenInclude(x => x.MonthlyCosts).AsNoTracking()
    //             .Include(x => x.Monthly)
    //                 .ThenInclude(x => x.RegularFeesPerUnit).AsNoTracking()
    //             .Include(x => x.Monthly)
    //                 .ThenInclude(x => x.ReducedFeesPerUnit).AsNoTracking()
    //             .Include(x => x.Monthly)
    //                 .ThenInclude(x => x.Daily)
    //                     .ThenInclude(x => x.FeesPerUnit).AsNoTracking()
    //         select p;
    //
    //     return await periods.ToListAsync();
    // }
    
    // public async Task<List<FeePeriod>> GetFeePeriodsAsync(DateOnly? fromDate = null, DateOnly? toDate = null)
    // {
    //     var ctx = await ctxFactory.CreateDbContextAsync();
    //
    //     var feePeriods = 
    //         from p in ctx.Periods
    //             .Include(x => x.MonthlyFees)
    //             .Include(x => x.HourlyFees)
    //         select p;
    //
    //     feePeriods = (fromDate, toDate) switch
    //     {
    //         ({ } f, { } t) => feePeriods.Where(x => x.Start >= f && x.End <= t),
    //         ({ } f, _) => feePeriods.Where(x => x.Start >= f),
    //         (_, { } t) => feePeriods.Where(x => x.End <= t),
    //         _ => feePeriods
    //     };
    //
    //     return await feePeriods.OrderBy(x => x.Start).ToListAsync();
    // }
    //
    // public async Task<FeePeriod?> GetFeePeriodAsync(DateOnly date)
    // {
    //     var ctx = await ctxFactory.CreateDbContextAsync();
    //
    //     var feePeriods = 
    //         from p in ctx.Periods.AsNoTracking()
    //             .Include(x => x.MonthlyFees).AsNoTracking()
    //             .Include(x => x.HourlyFees).AsNoTracking()
    //         where p.Start <= date && p.End <= date
    //         select p;
    //
    //     return await feePeriods.SingleOrDefaultAsync();
    // }

    public async Task<List<SpotPriceRecord>> GetSpotPricesAsync(DateTimeOffset fromHour, DateTimeOffset toHour)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var prices =
            from p in ctx.Prices
            where p.Hour >= fromHour && p.Hour < toHour
            orderby p.Hour
            select p;

        return await prices.ToListAsync();
    }

    public async Task<SpotPriceRecord?> GetSpotPriceAsync(DateTimeOffset hour)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var prices =
            from p in ctx.Prices.AsNoTracking()
            where p.Hour == hour
            select p;

        return await prices.SingleOrDefaultAsync();
    }

    // public async Task AddOrUpdatePricesAsync(IEnumerable<SpotPriceRecord> spotPrices)
    // {
    //     var ctx = await ctxFactory.CreateDbContextAsync();
    //
    //     var list = spotPrices.OrderBy(x => x.Hour).ToList();
    //     if (list.Count > 0)
    //     {
    //         var now = DateTimeOffset.Now;
    //         
    //         var min = list[0].Hour;
    //         var max = list[^1].Hour;
    //
    //         var periods = await GetFeePeriodsAsync(min.ToDateOnly(), max.ToDateOnly());
    //
    //         var pricesToUpdate = (await GetSpotPricesAsync(min, max)).ToImmutableSortedDictionary(x => x.Hour, x => x);
    //
    //         foreach (var newPrice in list)
    //         {
    //             var periodId = periods.SingleOrDefault(x =>
    //                 x.Start >= newPrice.Hour.ToDateOnly() && x.End >= newPrice.Hour.ToDateOnly())?.Id;
    //             
    //             if (pricesToUpdate.TryGetValue(newPrice.Hour, out var priceToUpdate))
    //             {
    //                 bool needsToUpdate = priceToUpdate.RawPrice != newPrice.RawPrice ||
    //                                      (!newPrice.IsPrediction &&
    //                                       newPrice.IsPrediction != priceToUpdate.IsPrediction);
    //
    //                 if (!needsToUpdate) continue;
    //                 
    //                 priceToUpdate.RawPrice = newPrice.RawPrice;
    //                 priceToUpdate.Source = newPrice.Source;
    //                 priceToUpdate.FeePeriodId = periodId;
    //                 priceToUpdate.IsPrediction = newPrice.IsPrediction;
    //                 priceToUpdate.LastUpdated = now;
    //             }
    //             else
    //             {
    //                 newPrice.FeePeriodId = periodId;
    //                 newPrice.LastUpdated = now;
    //
    //                 await ctx.Prices.AddAsync(newPrice);
    //             }
    //         }
    //
    //         await ctx.SaveChangesAsync();
    //     }
    // }

    public async Task<List<EnergyUsageRecord>> GetConsumptionForHourAsync(DateTimeOffset hour, string? entityId = null)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var consumptions =
            from c in ctx.Usages.AsNoTracking()
            where c.Hour == hour
            select c;

        if (entityId is not null)
        {
            consumptions = consumptions.Where(x => x.EntityId == entityId);
        }

        return await consumptions.OrderBy(x => x.EntityId).ToListAsync();
    }

    public async Task<List<EnergyUsageRecord>> GetConsumptionForDayAsync(DateOnly day, string? entityId = null)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();

        var fromTime = day.StartTime();
        var endTime = day.EndTime();
        
        var consumptions =
            from c in ctx.Usages.AsNoTracking()
            where c.Hour >= fromTime && c.Hour < endTime
            select c;

        if (entityId is not null)
        {
            consumptions = consumptions.Where(x => x.EntityId == entityId);
        }

        return await consumptions.OrderBy(x => x.EntityId).ToListAsync();
    }
}