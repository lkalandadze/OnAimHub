using Consul;
using GameLib.Application;
using GameLib.Application.Configurations;
using GameLib.Application.Controllers;
using GameLib.Application.Generators;
using GameLib.Application.Holders;
using GameLib.Application.Managers;
using GameLib.Application.Services.Abstract;
using GameLib.Application.Services.Concrete;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using GameLib.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Application;
using Shared.Application.Configurations;
using Shared.Application.Middlewares;
using Shared.Domain.Abstractions.Repository;
using Shared.Lib.SwaggerFilters;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace GameLib.ServiceRegistry;

public static class DependencyResolver
{
    public static IServiceCollection ResolveGameLibServices<TGameConfiguration>(this IServiceCollection services, IConfiguration configuration, List<Type> prizeGroupTypes)
        where TGameConfiguration : GameConfiguration<TGameConfiguration>
    {
        var apiName = configuration["GameInfoConfiguration:ApiName"];

        Globals.ConfigurationType = typeof(TGameConfiguration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<GameSettings>();
        services.AddSingleton<GeneratorHolder>();
        services.AddSingleton<ConfigurationHolder>();
        services.AddSingleton<RepositoryManager>();
        services.AddSingleton<EntityGenerator>();
        services.AddSingleton<CommandGenerator>();

        services.AddSingleton(prizeGroupTypes);

        foreach (var type in prizeGroupTypes)
        {
            services.AddScoped(typeof(IPrizeGroupRepository<>).MakeGenericType(type), typeof(PrizeGroupRepository<>).MakeGenericType(type));
        }

        services.AddHttpClient();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddScoped<IHubService, HubService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        //services.AddScoped<IPrizeTypeRepository, PrizeTypeRepository>();
        services.AddScoped<IGameConfigurationRepository, GameConfigurationRepository>();
        services.AddScoped<IPrizeHistoryRepository, PrizeHistoryRepository>();
        services.AddScoped<ISettingRepository, GameSettingRepository<TGameConfiguration>>();

        services.AddScoped<IConsulClient, ConsulClient>();
        services.AddScoped<IConsulGameService, ConsulGameService>();
        services.AddScoped<IGameConfigurationService, GameConfigurationService>();
        //services.AddScoped<IPrizeTypeService, PrizeTypeService>();
        services.AddScoped<IGameService, GameService>();

        services.Configure<GameInfoConfiguration>(configuration.GetSection("GameInfoConfiguration"));
        services.Configure<HubApiConfiguration>(configuration.GetSection("HubApiConfiguration"));
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.Configure<BasicAuthConfiguration>(configuration.GetSection("BasicAuthConfiguration"));
        services.Configure<PrizeGenerationConfiguration>(configuration.GetSection("PrizeGenerationConfiguration"));

        ConfigureSwagger(services, apiName);
        ConfigureAuthentication(services, configuration);

        services.AddAuthentication("BasicAuth");
        //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);

        services.AddHttpContextAccessor();
        services.AddAuthorization();
        services.AddControllers(options =>
        {
            options.Conventions.Add(new RoutePrefixConvention(apiName));
        })
            .AddApplicationPart(typeof(BaseApiController).Assembly).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.Converters.Add(new GameConfigurationJsonConverter<TGameConfiguration>());
        });

        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();

        HandleInitializations(services);

        return services;
    }

    public static void ResolveGameLib(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        var apiName = configuration["GameInfoConfiguration:ApiName"];

        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        ConfigureSwagger(app, env, apiName);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static void HandleInitializations(IServiceCollection services)
    {
        services.BuildServiceProvider().GetRequiredService<RepositoryManager>();
        services.BuildServiceProvider().GetRequiredService<GameSettings>();
        services.BuildServiceProvider().GetRequiredService<GeneratorHolder>();
    }

    private static void ConfigureSwagger(IServiceCollection services, string routePrefix)
    {
        services.AddSwaggerGen(c =>
        {
            List<string> swaggerEndpoints = ["hub", "admin", "game"];

            swaggerEndpoints.ForEach(controller =>
            {
                c.SwaggerDoc(controller, new OpenApiInfo { Title = $"{controller.ToUpper()} | {routePrefix}", Version = "v1" });
            });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                var groupName = apiDesc.GroupName;
                return docName.Equals(groupName);
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the bearer scheme.",
            });

            c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Basic Authentication with username and password. Example: 'username:password' base64-encoded."
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
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Basic"
                        },
                        Scheme = "basic",
                        Name = "Basic",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            });

            c.SchemaFilter<PolymorphismSchemaFilter<GameConfiguration>>();
        });
    }

    private static void ConfigureSwagger(IApplicationBuilder app, IWebHostEnvironment env, string routePrefix)
    {
        app.UseSwagger();

        List<string> swaggerEndpoints = ["hub", "admin", "game"];

        swaggerEndpoints.ForEach(controller => app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{controller}/swagger.json", $"{controller.ToUpper()} | {routePrefix}");
            c.RoutePrefix = $"swagger/{controller}";
            c.DocumentTitle = $"{controller.ToUpper()} | {routePrefix}";
        }));

        if (env.IsDevelopment())
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger/game/index.html");
                    return;
                }

                if (context.Request.Path == "/swagger")
                {
                    context.Response.Redirect("/swagger/game/index.html");
                    return;
                }

                await next();
            });
        }
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;

        var ecdsa = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(jwtConfig.PublicKey);
        ecdsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
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