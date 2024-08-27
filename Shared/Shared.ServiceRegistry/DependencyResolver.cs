using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Application.Configurations;
using Shared.Application.Holders;
using Shared.Application.Managers;
using Shared.Application.Options;
using Shared.Application.Services.Abstract;
using Shared.Application.Services.Concrete;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.Repositories;
using System.Security.Cryptography;

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

        services.AddSingleton<IAuthService, AuthService>();
        services.AddScoped<IHubService, HubService>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGameVersionRepository, GameVersionRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();
        services.AddScoped<IPrizeHistoryRepository, PrizeHistoryRepository>();

        services.BuildServiceProvider().GetRequiredService<RepositoryManager>();

        services.AddHostedService<PrizeConfiguratorService>();
        services.AddScoped<HttpClient>();

        services.Configure<HubApiConfiguration>(configuration.GetSection("HubApiConfiguration"));
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.Configure<PrizeGenerationSettings>(configuration.GetSection("PrizeGenerationSettings"));

        ConfigureSwagger(services);
        ConfigureJwt(services, configuration);

        services.AddHttpContextAccessor();
        services.AddAuthorization();

        return services;
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
        });
    }

    private static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;

        var ecdsa = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(jwtConfig.PublicKey);
        ecdsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new ECDsaSecurityKey(ecdsa),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}