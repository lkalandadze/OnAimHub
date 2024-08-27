using Consul;
using Hub.Api;
using Hub.Api.Common.Consul;
using Hub.Api.Middlewares;
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
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
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
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ITokenRecordRepository, TokenRecordRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IActiveGameService, ActiveGameService>();

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

        services.AddAuthorization();

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        //create database
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HubDbContext>();
            dbContext.Database.EnsureCreated();
        }

        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelExpandDepth(-1);
            c.DocumentTitle = "HubApi";
        });

        app.UseForwardedHeaders();
        app.UseCertificateForwarding();

        app.UseMiddleware<ErrorHandlerMiddleware>();

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

        var ecdsa = ECDsa.Create();
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
                Port = 8080, // The port your service is running on inside the container
                Tags = ["Hub"]
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
        services.AddHostedService<ConsulWatcherService>();
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