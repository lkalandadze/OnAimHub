using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Holders;
using Shared.Application.Options;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.DataAccess;
using Shared.Infrastructure.Repositories;

namespace Shared.ServiceRegistry;

public static class DependencyResolver
{
    public static IServiceCollection Resolve<T>(this IServiceCollection services, IConfiguration configuration)
        where T : SharedGameConfigDbContext
    {
        services.AddScoped<Configurator>();
        //services.AddScoped<IPrizeRepository, PrizeRepository>();
        //services.AddScoped<IPrizeGroupRepository, PrizeGroupRepository<T>>();

        services.Configure<PrizeGenerationSettings>(configuration.GetSection("PrizeGenerationSettings"));

        return services;
    }
}