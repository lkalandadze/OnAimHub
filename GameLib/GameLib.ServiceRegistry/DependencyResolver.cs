using Consul;
using GameLib.Application.Configurations;
using GameLib.Application.Holders;
using GameLib.Application.Managers;
using GameLib.Application.Services.Abstract;
using GameLib.Application.Services.Concrete;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Infrastructure.DataAccess;
using GameLib.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;

namespace GameLib.ServiceRegistry;

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

        services.AddHttpClient();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddScoped<IHubService, HubService>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<IPrizeHistoryRepository, PrizeHistoryRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IConsulClient, ConsulClient>();
        services.AddScoped<IConsulGameService, ConsulGameService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();

        services.BuildServiceProvider().GetRequiredService<RepositoryManager>();

        services.AddHostedService<PrizeConfiguratorService>();
        
        services.Configure<HubApiConfiguration>(configuration.GetSection("HubApiConfiguration"));
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.Configure<PrizeGenerationConfiguration>(configuration.GetSection("PrizeGenerationConfiguration"));

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
    private static bool IsRunningInDocker()
    {
        var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
    }
}