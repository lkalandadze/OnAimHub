using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Persistance.Data.Hub;
using OnAim.Admin.Infrasturcture.Persistance.Data.LeaderBoard;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;

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

        return services;
    }

    private static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("MongoDB");
        services.AddScoped<AuditLogDbContext>().AddOptions<MongoDbOptions>().Bind(options);
    }
}