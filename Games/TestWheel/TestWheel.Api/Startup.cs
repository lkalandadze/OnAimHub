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
using TestWheel.Application.Services.Abstract;
using TestWheel.Application.Services.Concrete;
using TestWheel.Domain.Abstractions.Repository;
using TestWheel.Domain.Entities;
using TestWheel.Infrastructure.DataAccess;
using Wheel.Infrastructure.Repositories;

namespace TestWheel.Api;

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

        services.AddScoped<SharedGameConfigDbContext, TestWheelConfigDbContext>();
        services.AddScoped<SharedGameConfigDbContext<TestWheelConfiguration>, TestWheelConfigDbContext>();

        services.AddDbContext<TestWheelConfigDbContext>(opt =>
        {
            opt.UseNpgsql(Configuration.GetConnectionString("GameConfig"));
        });

        services.AddDbContext<SharedGameConfigDbContext>(opt =>
        {
            opt.UseNpgsql(Configuration.GetConnectionString("GameConfig"));
        });

        services.AddDbContext<SharedGameHistoryDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("GameHistory")));

        services.AddScoped<IRoundRepository, RoundRepository>();
        services.AddScoped<ITestWheelPrizeRepository, TestWheelPrizeRepository>();
        services.AddScoped<ITestWheelConfigurationRepository, TestWheelConfigurationRepository>();
        services.AddScoped<IMessageBus, MessageBus>();

        var prizeGroupTypes = new List<Type> { typeof(Round), typeof(JackpotPrizeGroup) };
        services.ResolveGameLibServices<TestWheelConfiguration>(Configuration, prizeGroupTypes);

        services.AddScoped<ITestWheelService, TestWheelService>();

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
                Name = "testwheelapi",
                Address = "testwheelapi",
                Port = 8080,
                Tags = ["Game", "Back"],
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
        //TODO
        //services.Configure<ConsulConfig>(Configuration.GetSection("Consul"));
        //services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        //{
        //    var address = Configuration["Consul:Host"];
        //    consulConfig.Address = new Uri(address!);
        //}));

        //services.AddHostedService<ConsulHostedService>();
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