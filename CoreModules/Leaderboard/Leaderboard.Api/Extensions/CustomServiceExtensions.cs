using Consul;
using Leaderboard.Api.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Leaderboard.Api.Extensions;

public static class CustomServiceExtensions
{
    public static void ConfigureSwagger(IServiceCollection services)
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

    public static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
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

    public static void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        if (IsRunningInDocker())
        {
            ConfigureConsulLifetime(app, lifetime);
        }
        app.UseCors("AllowAll");
    }

    private static void ConfigureConsulLifetime(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        if (!IsRunningInDocker())
        {
            Console.WriteLine("Skipping Consul registration because the application is not running in Docker.");
            return;
        }

        var serviceId = Guid.NewGuid().ToString();

        lifetime.ApplicationStarted.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = "leaderboardapi",
                Address = "leaderboardapi",
                Port = 8080,
                Tags = new[] { "Leaderboard", "Back" }, // Use array initialization for the Tags property
                Meta = new Dictionary<string, string>
            {
                { "LeaderboardData", "Leaderboard" }
            }
            };

            consulClient.Agent.ServiceRegister(registration).Wait();
        });

        lifetime.ApplicationStopped.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            consulClient.Agent.ServiceDeregister(serviceId).Wait();
        });
    }

    public static void ConfigureConsul(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConsulConfig>(configuration.GetSection("Consul"));
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["Consul:Host"];
            consulConfig.Address = new Uri(address!);
        }));

        services.AddHostedService<ConsulHostedService>();
    }

    private static bool IsRunningInDocker()
    {
        var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
    }
    public class JwtConfiguration
    {
        public string PublicKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

}