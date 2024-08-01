using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Holders;
using Shared.Application.Options;
using Shared.Infrastructure.DataAccess;

namespace Shared.ServiceRegistry;

public static class DependencyResolver
{
    public static IServiceCollection Resolve(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GameConfigDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddSingleton<GeneratorHolder>();
        services.Configure<PrizeGenerationSettings>(configuration.GetSection("PrizeGenerationSettings"));

        return services;
    }
}