using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.Infrasturcture.Persistance.Data;
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

        services.AddMongoDbContext(configuration);

        return services;
    }

    private static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("MongoDB");
        services.AddScoped<AuditLogDbContext>().AddOptions<MongoDbOptions>().Bind(options);
    }
}
