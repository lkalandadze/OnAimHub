using Consul;
using Hub.Api.Common.Consul;
using Hub.Application;
using Hub.Application.Configurations;
using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Concrete;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Infrastructure.DataAccess;
using Hub.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var env = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();

        services.AddDbContext<HubDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<HttpClient>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtConfiguration>(Configuration.GetSection("JwtConfiguration"));
        services.Configure<CasinoApiConfiguration>(Configuration.GetSection("CasinoApiConfiguration"));

        ConfigureMassTransit(services, Configuration, env);

        services.AddMediatR(new[]
        {
            typeof(CreateAuthenticationTokenHandler).GetTypeInfo().Assembly,
        });

        services.AddMassTransitHostedService();

        services.AddLogging();
        ConfigureLogging();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }

        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        ConfigureSwagger(services);
        ConfigureJwt(services);
        ConfigureApplicationContext(services);

        services.AddAuthorization();

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.DefaultModelExpandDepth(-1));

        app.UseForwardedHeaders();
        app.UseCertificateForwarding();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        if (IsRunningInDocker())
        {
            ConfigureConsulLifetime(app, lifetime);
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void ConfigureLogging()
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "message", new RenderedMessageColumnWriter() },
            { "message_template", new MessageTemplateColumnWriter() },
            { "level", new LevelColumnWriter() },
            { "timestamp", new TimestampColumnWriter() },
            { "exception", new ExceptionColumnWriter() },
            { "properties", new PropertiesColumnWriter() },
            { "props_test", new SinglePropertyColumnWriter("UserName", PropertyWriteMethod.ToString, NpgsqlTypes.NpgsqlDbType.Varchar, "UserName") }
        };

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.PostgreSQL(
                connectionString: Configuration.GetConnectionString("DefaultConnection"),
                tableName: "Logs",
                needAutoCreateTable: true,
                columnOptions: columnWriters
            )
            .CreateLogger();
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

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hub Api",
                Version = "v1",
                Description = "Hub Api",
                Contact = new OpenApiContact
                {
                    Name = "HubApi",
                },
            });
            c.ResolveConflictingActions(apiDescription => apiDescription.First());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the bearer scheme.",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
        });
    }

    private void ConfigureJwt(IServiceCollection services)
    {
        var jwtConfig = Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;

        ECDsa ecdsa = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(jwtConfig.PrivateKey);
        ecdsa.ImportECPrivateKey(keyBytes, out _);

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
                };
            });
    }

    private void ConfigureConsulLifetime(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        lifetime.ApplicationStarted.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "hubapi",
                Address = "hubapi", // Docker service name or external IP address
                Port = 8080 // The port your service is running on inside the container
            };
            consulClient.Agent.ServiceRegister(registration).Wait();
        });

        // Deregister the service from Consul when application stops
        lifetime.ApplicationStopped.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "hubapi"
            };
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
    }

    private void ConfigureConsul(IServiceCollection services)
    {
        services.Configure<ConsulConfig>(Configuration.GetSection("Consul"));
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configuration["Consul:Host"]!);
        }));

        services.AddHostedService<ConsulHostedService>();
    }

    private bool IsRunningInDocker()
    {
        var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
    }

    private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQSettings:Host"], h =>
                {
                    h.Username(configuration["RabbitMQSettings:User"]);
                    h.Password(configuration["RabbitMQSettings:Password"]);
                });

                cfg.ReceiveEndpoint($"{configuration["RabbitMQSettings:QueueName"]}_{env.EnvironmentName}_TEMP", ep =>
                {
                    // ep.ConfigureConsumer<YourConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}