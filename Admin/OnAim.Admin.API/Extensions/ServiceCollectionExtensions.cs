using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnAim.Admin.API.Service.Endpoint;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OnAim.Admin.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static TOptions BindOptions<TOptions>(
    this IConfiguration configuration,
    Action<TOptions>? configurator = null
    )
     where TOptions : new()
    {
        return BindOptions(configuration, typeof(TOptions).Name, configurator);
    }

    public static TOptions BindOptions<TOptions>(
    this IConfiguration configuration,
    string section,
    Action<TOptions>? configurator = null
    )
    where TOptions : new()
    {
        var options = new TOptions();

        var optionsSection = configuration.GetSection(section);
        optionsSection.Bind(options);

        configurator?.Invoke(options);

        return options;
    }

    public static AuthenticationBuilder AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        var jwtOptions = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

        if (jwtOptions == null)
        {
            throw new ArgumentNullException(nameof(jwtOptions), "JWT configuration could not be loaded.");
        }

        if (string.IsNullOrWhiteSpace(jwtOptions.Secret))
        {
            throw new ArgumentException("SecretKey cannot be null or empty.", nameof(jwtOptions.Secret));
        }

        services.TryAddTransient<IJwtFactory, JwtFactory>();

        return services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = jwtOptions.Audience;
                options.SaveToken = true;
                options.RefreshOnIssuerKeyNotFound = false;
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    SaveSigninToken = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            policyBuilder.RequireAuthenticatedUser();

            options.DefaultPolicy = policyBuilder.Build();
        });
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IEndpointService, EndpointService>();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "OnAim", Description = "Docs OnAim API", Version = "3.0.0" });
            options.CustomSchemaIds(x => x.FullName);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}