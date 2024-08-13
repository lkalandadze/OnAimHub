﻿using Consul;
using Hub.Api.Consul;
using Hub.Application;
using Hub.Application.Configurations;
using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.Application.Helpers;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Concrete;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Infrastructure.DataAccess;
using Hub.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var appsettings = new AppSettings();
        Configuration.Bind(appsettings);
        BindJwtConfigValidAudiences(Configuration, appsettings);

        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAnyOrigin",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        services.AddSingleton(appsettings);
        services.AddHttpContextAccessor();

        services.AddDbContext<HubDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<HttpClient>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<CasinoApiConfiguration>(Configuration.GetSection("CasinoApiConfiguration"));

        services.AddMediatR(new[]
        {
            typeof(CreateAuthenticationTokenHandler).GetTypeInfo().Assembly,
        });

        services.AddLogging();
        ConfigureLogging();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }

        ConfgiureJwt(services, appsettings);
        ConfigureApplicationContext(services);

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        ConfigureSwagger(services);

        services.AddSwaggerGen();

        services.Configure<ForwardedHeadersOptions>(o =>
        {
            o.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            o.KnownNetworks.Clear();
            o.KnownProxies.Clear();
        });

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

    private static void BindJwtConfigValidAudiences(IConfiguration configuration, AppSettings appSettings)
    {
        var section = configuration.GetSection("jwtconfig:ValidAudiences");

        if (section != null && section.Value != null)
        {
            var values = section.Get<string>()?.Split(',').ToList();
            appSettings.JwtConfig.ValidAudiences = values;
        }
    }

    public static void ConfgiureJwt(IServiceCollection services, AppSettings appSettings)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Audience = appSettings.JwtConfig.ValidAudience;
            options.TokenValidationParameters = TokenHelper.GetTokenValidationParameters(appSettings);
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("IS-TOKEN-EXPIRED", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    private void ConfigureApplicationContext(IServiceCollection services)
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
                            var username = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                            var playerId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "PlayerId")?.Value;

                            applicationContext.PlayerId = int.Parse(playerId!);
                            applicationContext.UserName = username;
                        }
                    }
                }
            }

            return applicationContext;
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
}