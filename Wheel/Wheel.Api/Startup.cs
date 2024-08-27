using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess;
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
        services.AddDbContext<WheelConfigDbContext>(opt =>
             opt.UseNpgsql(Configuration.GetConnectionString("GameConfig")));

        services.AddDbContext<SharedGameHistoryDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("GameHistory")));

        services.AddScoped<SharedGameConfigDbContext, WheelConfigDbContext>();

        var prizeGroupTypes = new List<Type> { typeof(WheelPrizeGroup), typeof(JackpotPrizeGroup) };
        services.Resolve(Configuration, prizeGroupTypes);

        services.AddSingleton(prizeGroupTypes);
        services.AddScoped<GameManager>();
        
        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAnyOrigin",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
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
        {
            ConfigureConsulLifetime(app, lifetime);
        }

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