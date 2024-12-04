using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Infrastructure.DataAccess;
using Leaderboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

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

        var redisConnectionString = configuration.GetConnectionString("Redis");
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILeaderboardScheduleRepository, LeaderboardScheduleRepository>();
        services.AddScoped<ILeaderboardRecordRepository, LeaderboardRecordRepository>();
        services.AddScoped<ILeaderboardProgressRepository, LeaderboardProgressRepository>();
        services.AddScoped<ILeaderboardResultRepository, LeaderboardResultRepository>();
        return services;
    }
}