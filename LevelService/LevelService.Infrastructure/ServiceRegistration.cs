using LevelService.Infrastructure.DataAccess;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LevelService.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddDbContext<LevelDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OnAimLevelService"), x =>
            {
                x.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
            }).LogTo(Console.WriteLine, LogLevel.Information);
        }, ServiceLifetime.Transient);

        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();

        return services;
    }
}