using Consul;
using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Infrastructure.DataAccess;
using GameLib.ServiceRegistry;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.DataAccess;
using Shared.Infrastructure.MassTransit;
using Shared.IntegrationEvents.IntegrationEvents.Player;
using Wheel.Api.Consul;
using Wheel.Application.Services.Abstract;
using Wheel.Application.Services.Concrete;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;
using Wheel.Infrastructure.Repositories;

namespace Wheel.Api;

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
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetConfigurationByIdQueryHandler).Assembly));

        services.AddScoped<SharedGameConfigDbContext, WheelConfigDbContext>();
        services.AddScoped<SharedGameConfigDbContext<WheelConfiguration>, WheelConfigDbContext>();

        services.AddDbContext<WheelConfigDbContext>(opt =>
        {
            opt.UseNpgsql(Configuration.GetConnectionString("GameConfig"));
        });

        services.AddDbContext<SharedGameConfigDbContext>(opt =>
        {
            opt.UseNpgsql(Configuration.GetConnectionString("GameConfig"));
        });

        services.AddDbContext<SharedGameHistoryDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("GameHistory")));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        services.AddScoped<IWheelPrizeGroupRepository, WheelPrizeGroupRepository>();
        services.AddScoped<IWheelPrizeRepository, WheelPrizeRepository>();
        services.AddScoped<IWheelConfigurationRepository, WheelConfigurationRepository>();
        services.AddScoped<IMessageBus, MessageBus>();

        var prizeGroupTypes = new List<Type> { typeof(WheelPrizeGroup) };
        services.ResolveGameLibServices<WheelConfiguration>(Configuration, prizeGroupTypes);

        services.AddScoped<IWheelService, WheelService>();

        ConfigureMassTransit(services, Configuration, env, consumerAssemblyMarkerType: typeof(Program));
        services.AddMassTransitHostedService();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        if (IsRunningInDocker())
        {
            ConfigureConsulLifetime(app, lifetime);
        }
        app.UseCors("AllowAll");
        app.ResolveGameLib(Configuration, env, lifetime);
    }

    private void ConfigureConsulLifetime(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var serviceId = Guid.NewGuid().ToString();

        lifetime.ApplicationStarted.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            GameResponseModel activeGameModel;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
                activeGameModel = gameService.GetGame();
            }

            var serializedGameData = JsonConvert.SerializeObject(activeGameModel);

            var registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = "wheelapi",
                Address = "wheelapi",
                Port = 8080,
                Tags = ["Wheel", "Back"],
                Meta = new Dictionary<string, string>
                {
                    { "GameData", serializedGameData }
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

    private void ConfigureConsul(IServiceCollection services)
    {
        services.Configure<ConsulConfig>(Configuration.GetSection("Consul"));
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = Configuration["Consul:Host"];
            consulConfig.Address = new Uri(address!);
        }));

        services.AddHostedService<ConsulHostedService>();
    }

    private bool IsRunningInDocker()
    {
        var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
    }

    private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env, Type consumerAssemblyMarkerType)
    {
        var rabbitMqOptions = configuration.GetSection("RabbitMQSettings").Get<RabbitMqOptions>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(consumerAssemblyMarkerType.Assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.User);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.Message<UpdatePlayerExperienceEvent>(c => c.SetEntityName("levels.fanout"));
                cfg.Publish<UpdatePlayerExperienceEvent>(p =>
                {
                    p.ExchangeType = "fanout";
                });
            });
        });
    }
}