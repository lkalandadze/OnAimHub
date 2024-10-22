using Consul;
using GameLib.Infrastructure.DataAccess;
using GameLib.ServiceRegistry;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Infrastructure.DataAccess;
using Wheel.Api.Consul;
using Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;
using Wheel.Application.Models.Game;
using Wheel.Application.Services.Abstract;
using Wheel.Application.Services.Concrete;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;
using Wheel.Infrastructure.Repositories;

namespace Wheel.Api;

//public class TestContext : SharedGameConfigDbContext<WheelConfiguration>
//{
//    public TestContext(DbContextOptions<TestContext> options)
//     : base(options)
//    {
//    }

//}

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }



    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetConfigurationByIdQueryHandler).Assembly));


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

        services.AddScoped<IRoundRepository, RoundRepository>();
        services.AddScoped<IWheelPrizeRepository, WheelPrizeRepository>();
        services.AddScoped<IWheelConfigurationRepository, WheelConfigurationRepository>();

        var prizeGroupTypes = new List<Type> { typeof(Round), typeof(JackpotPrizeGroup) };
        services.Resolve<WheelConfiguration>(Configuration, prizeGroupTypes, "WheelApi");

        services.AddScoped<IWheelService, WheelService>();


        ConfigureMassTransit(services);
        services.AddMassTransitHostedService();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        if (IsRunningInDocker())
        {
            ConfigureConsulLifetime(app, lifetime);
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelExpandDepth(-1);
            c.DocumentTitle = "Wheel.Api";
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
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
                var gameService = scope.ServiceProvider.GetRequiredService<IWheelService>();
                activeGameModel = gameService.GetGame();
            }

            var serializedGameData = JsonConvert.SerializeObject(activeGameModel);

            var registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = "wheelapi",
                Address = "wheelapi",

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

    private void ConfigureMassTransit(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(Configuration["RabbitMQSettings:Host"], h =>
                {
                    h.Username(Configuration["RabbitMQSettings:User"]!);
                    h.Password(Configuration["RabbitMQSettings:Password"]!);
                });

                cfg.ReceiveEndpoint($"{Configuration["RabbitMQSettings:QueueName"]}_{"EnvironmentName"}_TEMP", ep =>
                {
                    // ep.ConfigureConsumer<YourConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}