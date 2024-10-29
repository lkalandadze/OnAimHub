using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MissionService.Domain.Abstractions.Repository;
using MissionService.Infrastructure.DataAccess;
using MissionService.Infrastructure.Repositories;

namespace MissionService.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddDbContext<MissionDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OnAimMission"), x =>
            {
                x.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
            }).LogTo(Console.WriteLine, LogLevel.Information);
        }, ServiceLifetime.Transient);

        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();

        return services;
    }
}