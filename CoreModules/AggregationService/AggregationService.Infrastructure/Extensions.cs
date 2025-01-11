using AggregationService.Application.Services.Abstract;
using AggregationService.Application.Services.Concrete;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.Data;
using AggregationService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Infrastructure.Bus;
using StackExchange.Redis;

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

        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDBConnectionString");
            return new MongoClient(mongoConnectionString);
        });


        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });


        services
            .AddScoped<IMessageBus, MessageBus>()
            .AddScoped<IAggregationConfigurationRepository, AggregationConfigurationRepository>()
            .AddScoped<IFilterRepository, FilterRepository>()
            .AddScoped<IPointEvaluationRuleRepository, PointEvaluationRuleRepository>()
            .AddScoped<IConfigurationStore, ConfigurationStore>()
            ;

        return services;
    }
}
