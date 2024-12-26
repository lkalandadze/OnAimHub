using AggregationService.Application.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AggregationService.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAggregationService, Application.Services.Concrete.AggregationService>();

        return services;
    }
}
