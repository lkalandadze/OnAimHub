using Leaderboard.Domain.Abstractions;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Infrastructure.DataAccess;
using Leaderboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leaderboard.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddDbContext<LeaderboardDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OnAimLeaderboard"), x =>
            {
                x.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
            }).LogTo(Console.WriteLine, LogLevel.Information);
        }, ServiceLifetime.Transient);

        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILeaderboardTemplateRepository, LeaderboardTemplateRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}