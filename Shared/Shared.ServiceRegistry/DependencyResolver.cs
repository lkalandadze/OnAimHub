using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Holders;
using Shared.Application.Managers;
using Shared.Application.Options;
using Shared.Application.Services;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.Repositories;

namespace Shared.ServiceRegistry;

public static class DependencyResolver
{
    public static IServiceCollection Resolve(this IServiceCollection services, IConfiguration configuration, List<Type> prigeGroupTypes)
    {
        services.AddSingleton<GeneratorHolder>();
        services.AddSingleton<ConfigurationHolder>();
        services.AddSingleton<RepositoryManager>();

        foreach (var type in prigeGroupTypes)
        {
            services.AddScoped(typeof(IPrizeGroupRepository<>).MakeGenericType(type), typeof(PrizeGroupRepository<>).MakeGenericType(type));
        }

        services.BuildServiceProvider().GetRequiredService<RepositoryManager>();

        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGameVersionRepository, GameVersionRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();
        services.AddScoped<IPrizeHistoryRepository, PrizeHistoryRepository>();

        //TODO REMOVE
        //services.AddHostedService<PrizeConfiguratorService>();
        //services.AddHostedService<UpdatePrizeGroupService>();

        services.Configure<PrizeGenerationSettings>(configuration.GetSection("PrizeGenerationSettings"));

        return services;
    }
}