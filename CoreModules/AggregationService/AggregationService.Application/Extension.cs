using AggregationService.Application.Services.Abstract;
using AggregationService.Application.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AggregationService.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAggregationConfigurationService, AggregationConfigurationService>();

        return services;
    }
}
