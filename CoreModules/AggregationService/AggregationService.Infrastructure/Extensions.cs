using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.MongoDB;
using AggregationService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.Infrastructure.Bus;
using StackExchange.Redis;

namespace AggregationService.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IMongoClient>(sp =>
        //{
        //    var mongoConnectionString = configuration.GetConnectionString("MongoDBConnectionString");
        //    return new MongoClient(mongoConnectionString);
        //});


        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddMongoDbContext(configuration);


        services
            .AddScoped<IMessageBus, MessageBus>()
            .AddScoped<IAggregationConfigurationRepository, AggregationConfigurationRepository>()
            .AddScoped<IFilterRepository, FilterRepository>()
            .AddScoped<IPointEvaluationRuleRepository, PointEvaluationRuleRepository>()
            .AddScoped<IConfigurationStore, ConfigurationStore>()
            ;

        return services;
    }
    private static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDB");
        services.AddScoped<AggregationDbContext>().AddOptions<MongoDbOptions>().Bind(mongoDbSettings);
        var mongoDbConnectionString = mongoDbSettings["Connection"];
        var mongoDbDatabaseName = mongoDbSettings["DatabaseName"];

        if (string.IsNullOrEmpty(mongoDbConnectionString))
        {
            throw new ArgumentNullException(nameof(mongoDbConnectionString), "MongoDB connection string is not configured.");
        }

        if (string.IsNullOrEmpty(mongoDbDatabaseName))
        {
            throw new ArgumentNullException(nameof(mongoDbDatabaseName), "MongoDB database name is not configured.");
        }

        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDbConnectionString));
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbDatabaseName);
        });

        services.AddScoped<AggregationDbContext>();
    }
}
