using Consul;
using Hangfire;
using Hangfire.PostgreSql;
using Hub.Api;
using Hub.Api.Common.Consul;
using Hub.Api.Middlewares;
using Hub.Application;
using Hub.Application.Configurations;
using Hub.Application.Features.IdentityFeatures.Commands.BasicAuthentication;
using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Application.Services.Concrete;
using Hub.Application.Services.Concrete.BackgroundJobs;
using Hub.Application.Services.Concretel;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Infrastructure.DataAccess;
using Hub.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
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
            options.UseNpgsql(Configuration.GetConnectionString("OnAimHub")));

        services.AddHttpClient();

        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IPlayerBalanceRepository, PlayerBalanceRepository>();
        services.AddScoped<ITokenRecordRepository, TokenRecordRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IConsulLogRepository, ConsulLogRepository>();
        services.AddScoped<IPlayerProgressRepository, PlayerProgressRepository>();
        services.AddScoped<IPlayerProgressHistoryRepository, PlayerProgressHistoryRepository>();
        services.AddScoped<ISegmentRepository, SegmentRepository>();
        services.AddScoped<IPlayerSegmentRepository, PlayerSegmentRepository>();
        services.AddScoped<IPlayerLogRepository, PlayerLogRepository>();
        services.AddScoped<IPlayerSegmentActRepository, PlayerSegmentActRepository>();
        services.AddScoped<IPlayerSegmentActHistoryRepository, PlayerSegmentActHistoryRepository>();
        services.AddScoped<IPlayerBlockedSegmentRepository, PlayerBlockedSegmentRepository>();
        services.AddScoped<IConsulLogRepository, ConsulLogRepository>();
        services.AddScoped<IReferralDistributionRepository, ReferralDistributionRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IPlayerBanRepository, PlayerBanRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IActiveGameService, ActiveGameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerBalanceService, PlayerBalanceService>();
        services.AddScoped<IPlayerProgressService, PlayerProgressService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPlayerSegmentService, PlayerSegmentService>();
        services.AddScoped<IPlayerSegmentActService, PlayerSegmentActService>();
        services.AddScoped<IPlayerBlockedSegmentService, PlayerBlockedSegmentService>();
        services.AddScoped<IPlayerLogService, PlayerLogService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IBackgroundJobScheduler, HangfireJobScheduler>();

        services.Configure<JwtConfiguration>(Configuration.GetSection("JwtConfiguration"));
        services.Configure<BasicAuthConfiguration>(Configuration.GetSection("BasicAuthConfiguration"));
        services.Configure<CasinoApiConfiguration>(Configuration.GetSection("CasinoApiConfiguration"));
        services.Configure<GameApiConfiguration>(Configuration.GetSection("GameApiConfiguration"));

        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(Configuration.GetConnectionString("OnAimHub")));
        services.AddHangfireServer();

        services.AddHostedService<JobSyncService>();

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
        ConfigureAuthentication(services);

        services.AddAuthentication("BasicAuth")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);

        services.AddAuthorization();

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IBackgroundJobClient backgroundJobs)
    {
        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var settingRepository = scope.ServiceProvider.GetRequiredService<ISettingRepository>();
            DbSettings.Init(settingRepository);
        }

        ConfigureSwagger(app);

        app.UseForwardedHeaders();
        app.UseCertificateForwarding();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard();
        backgroundJobs.Enqueue(() => Console.WriteLine("Hangfire initialized!"));

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
            c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
            c.SwaggerDoc("game", new OpenApiInfo { Title = "Game API", Version = "v1" });
            c.SwaggerDoc("hub", new OpenApiInfo { Title = "Hub API", Version = "v1" });

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
        });
    }

    private void ConfigureSwagger(IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/hub/swagger.json", "Hub | Hub.Api");
            c.RoutePrefix = "swagger";
            c.DocumentTitle = "Hub | Hub.Api";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/hub/swagger.json", "Hub | Hub.Api");

            c.RoutePrefix = "swagger/hub";
            c.DocumentTitle = "Hub | Hub.Api";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin | Hub.Api");

            c.RoutePrefix = "swagger/admin";
            c.DocumentTitle = "Admin | Hub.Api";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/game/swagger.json", "Game | Hub.Api");

            c.RoutePrefix = "swagger/game";
            c.DocumentTitle = "Game | Hub.Api";
        });
    }

    private void ConfigureAuthentication(IServiceCollection services)
    {
        var jwtConfig = Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()!;

        var ecdsa = ECDsa.Create();
        var keyBytes = Convert.FromBase64String(jwtConfig.PrivateKey);
        ecdsa.ImportECPrivateKey(keyBytes, out _);

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