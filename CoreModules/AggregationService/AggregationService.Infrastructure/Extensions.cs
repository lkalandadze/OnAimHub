using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.Data;
using AggregationService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AggregationService.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddDbContext<AggregationServiceContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"), x =>
            {
                x.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
            }).LogTo(Console.WriteLine, LogLevel.Information);
        }, ServiceLifetime.Transient);

        services
            .AddScoped<IAggregationConfigurationRepository, AggregationConfigurationRepository>()
            .AddScoped<IFilterRepository, FilterRepository>()
            .AddScoped<IPointEvaluationRuleRepository, PointEvaluationRuleRepository>()
            ;

        return services;
    }
}
