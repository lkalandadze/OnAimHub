using Consul;
using GameLib.Infrastructure.DataAccess;
using GameLib.ServiceRegistry;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Application.Models.Consul;
using Shared.Infrastructure.DataAccess;
using System.Reflection;
using Wheel.Api.Consul;
using Wheel.Application.Features.GameVersion.Commands.Update;
using Wheel.Application.Models.Game;
using Wheel.Application.Services.Abstract;
using Wheel.Application.Services.Concrete;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

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
        services.AddDbContext<WheelConfigDbContext>(opt =>
             opt.UseNpgsql(Configuration.GetConnectionString("GameConfig")));

        services.AddDbContext<SharedGameHistoryDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("GameHistory")));

        services.AddScoped<SharedGameConfigDbContext, WheelConfigDbContext>();

        var prizeGroupTypes = new List<Type> { typeof(WheelPrizeGroup), typeof(JackpotPrizeGroup) };
        services.Resolve(Configuration, prizeGroupTypes);

        services.AddSingleton(prizeGroupTypes);
        services.AddScoped<IGameService, GameService>();

        services.AddMediatR(new[]
{
            typeof(UpdateGameVersionCommandHandler).GetTypeInfo().Assembly,
        });

        ConfigureMassTransit(services);
        services.AddMassTransitHostedService();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        app.UseCors("AllowAnyOrigin");
        app.UseHttpsRedirection();

        if (IsRunningInDocker())
            ConfigureConsulLifetime(app, lifetime);

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wheel API V1");
            c.RoutePrefix = string.Empty;
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

            List<GameRegisterResponseModel> activeGameModel;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
                activeGameModel = gameService.GetGames();
            }

            var serializedGameData = JsonConvert.SerializeObject(activeGameModel);

            var registration = new AgentServiceRegistration()
            {
                ID = "wheelapi",
                Name = "wheelapi",
                Address = "wheelapi",
                Port = 8080,
                Tags = new[] { "Game", "Back" },
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