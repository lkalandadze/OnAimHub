using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Shared.Application;
using Shared.Application.Configurations;
using Shared.Application.Holders;
using Shared.Application.Managers;
using Shared.Application.Options;
using Shared.Application.Services;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
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

        services.BuildServiceProvider().GetRequiredService<RepositoryManager>();

        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGameVersionRepository, GameVersionRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();
        services.AddScoped<IPrizeHistoryRepository, PrizeHistoryRepository>();

        services.AddHostedService<PrizeConfiguratorService>();

        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.Configure<PrizeGenerationSettings>(configuration.GetSection("PrizeGenerationSettings"));

        ConfigureSwagger(services);
        ConfigureJwt(services, configuration);
        ConfigureApplicationContext(services);

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
        var jwtConfig = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

        RSA rsa = RSA.Create();
        string xmlKey = File.ReadAllText(jwtConfig.PublicKeyPath);
        rsa.FromXmlString(xmlKey);

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
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    private static void ConfigureApplicationContext(IServiceCollection services)
    {
        services.AddScoped(p =>
        {
            var applicationContext = new ApplicationContext();
            var accessor = p.GetService<IHttpContextAccessor>();

            if (accessor != null && accessor.HttpContext != null)
            {
                var authHeader = accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

                if (!string.IsNullOrEmpty(authHeader))
                {
                    var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

                    if (!string.IsNullOrEmpty(token))
                    {
                        var jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

                        if (jwtSecurityToken != null)
                        {
                            var playerId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "PlayerId")?.Value;
                            var userName = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;

                            applicationContext.PlayerId = int.Parse(playerId);
                            applicationContext.UserName = userName;
                        }
                    }
                }
            }

            return applicationContext;
        });
    }
}