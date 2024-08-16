using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions.Repository;
using Shared.Infrastructure.DataAccess;
using Shared.Infrastructure.Repositories;
using Shared.ServiceRegistry;
using Wheel.Api.Consul;
using Wheel.Application;
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
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        if (IsRunningInDocker())
        {
            ConfigureConsul(services);
        }

        services.AddDbContext<WheelConfigDbContext>(opt =>
             opt.UseNpgsql(Configuration.GetConnectionString("GameConfig")));

        services.AddDbContext<SharedGameHistoryDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("GameHistory")));

        services.AddScoped<SharedGameConfigDbContext, WheelConfigDbContext>();

        var prizeGroupTypes = new List<Type> { typeof(WheelPrizeGroup), typeof(JackpotPrizeGroup) };
        services.Resolve(Configuration, prizeGroupTypes);

        services.AddSingleton(prizeGroupTypes);

        services.AddScoped<GameManager>();
        services.AddScoped<IPriceRepository, PriceRepository>();

        ConfigureMassTransit(services);
        services.AddMassTransitHostedService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        if (IsRunningInDocker())
        {
            ConfigureConsulLifetime(app, lifetime);
        }

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

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

    private void ConfigureConsulLifetime(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        lifetime.ApplicationStarted.Register(() =>
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "wheelapi",
                Address = "wheelapi", // Docker service name or external IP address
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
                Name = "wheelapi"
            };
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
    }

    private void ConfigureConsul(IServiceCollection services)
    {
        services.Configure<ConsulConfig>(Configuration.GetSection("Consul"));
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = Configuration["Consul:Host"];
            consulConfig.Address = new Uri(address);
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