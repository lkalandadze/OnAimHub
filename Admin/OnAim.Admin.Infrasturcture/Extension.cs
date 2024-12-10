using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Persistance.Data.Hub;
using OnAim.Admin.Infrasturcture.Persistance.Data.LeaderBoard;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories;
using OnAim.Admin.Infrasturcture.Repository;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;
using OnAim.Admin.Infrasturcture.Repositories.Leaderboard;
using OnAim.Admin.Infrasturcture.Repositories.Promotion;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Interfaces;

namespace OnAim.Admin.Infrasturcture;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));
        });

        services.AddDbContext<ReadOnlyDataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("HubDefaultConnectionString"));
        });

        services.AddDbContext<LeaderBoardReadOnlyDataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("LeaderBoardDefaultConnectionString"));
        });

        services.AddMongoDbContext(configuration);

        services
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ICoinRepository, CoinRepository>()
            .AddScoped<IGameConfigurationTemplateRepository, GameConfigurationTemplateRepository>()
            .AddScoped<ILeaderboardTemplateRepository, LeaderboardTemplateRepository>()
            .AddScoped<IPromotionTemplateRepository, PromotionTemplateRepository>()
            .AddScoped<IPromotionViewTemplateRepository, PromotionViewTemplateRepository>()
            .AddScoped<ILeaderboardTemplateRepository, LeaderboardTemplateRepository>()
            .AddScoped<ILogRepository, LogRepository>()
            .AddScoped<IAppSettingRepository, AppSettingRepository>()
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IConfigurationRepository<>), typeof(ConfigurationRepository<>))
            .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
            .AddScoped(typeof(IPromotionRepository<>), typeof(PromotionRepository<>))
            .AddScoped(typeof(ILeaderBoardReadOnlyRepository<>), typeof(LeaderBoardReadOnlyRepository<>));
        ;

        return services;
    }
    private static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDB");
        services.AddScoped<AuditLogDbContext>().AddOptions<MongoDbOptions>().Bind(mongoDbSettings);
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

        services.AddScoped<AuditLogDbContext>();
    }
}