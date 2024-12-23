using AggregationService.Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AggregationService.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AggregationServiceContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));
        });      

        return services;
    }
}
