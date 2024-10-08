using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnAim.Admin.API.Service.Endpoint;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnAim.Admin.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static TOptions BindOptions<TOptions>(
        this IConfiguration configuration,
        Action<TOptions>? configurator = null
    ) where TOptions : new()
    {
        return BindOptions(configuration, typeof(TOptions).Name, configurator);
    }

    public static TOptions BindOptions<TOptions>(
        this IConfiguration configuration,
        string section,
        Action<TOptions>? configurator = null
    ) where TOptions : new()
    {
        var options = new TOptions();
        var optionsSection = configuration.GetSection(section);
        optionsSection.Bind(options);
        configurator?.Invoke(options);
        return options;
    }

    public static void AddCustomJwtAuthentication(this ContainerBuilder builder, IConfiguration configuration)
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

        builder.RegisterType<JwtFactory>().As<IJwtFactory>().InstancePerLifetimeScope();

        builder.Register(c =>
        {
            var options = new JwtBearerOptions
            {
                Audience = jwtOptions.Audience,
                SaveToken = true,
                RefreshOnIssuerKeyNotFound = false,
                RequireHttpsMetadata = false,
                IncludeErrorDetails = true,
                TokenValidationParameters = new TokenValidationParameters
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
                },
                Events = new JwtBearerEvents
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
                }
            };
            return options;
        }).SingleInstance();
    }

    public static void AddCustomAuthorization(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var services = c.Resolve<IServiceCollection>();
            services.AddAuthorization(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder();
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = policyBuilder.Build();
            });

            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }).AsSelf().SingleInstance();
    }

    public static void AddCustomCors(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var policyBuilder = new CorsPolicyBuilder();
            policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            return policyBuilder.Build();
        }).As<CorsPolicy>().SingleInstance();
    }

    public static void AddCustomServices(this ContainerBuilder builder)
    {
        builder.RegisterType<EndpointService>().As<IEndpointService>().InstancePerLifetimeScope();
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
    }

    public static void AddCustomSwagger(this ContainerBuilder builder)
    {
        builder.Register(c =>
        {
            var options = new SwaggerGenOptions();
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
            return options;
        }).SingleInstance();
    }
}
