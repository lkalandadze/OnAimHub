using Consul;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess;
using Shared.ServiceRegistry;
using Wheel.Infrastructure.DataAccess;
using Wheel.Domain.Entities;
using Wheel.Api.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

if (IsRunningInDocker())
{
    ConfigureConsul();
}

builder.Services.AddDbContext<WheelConfigDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("GameConfig")));

builder.Services.AddDbContext<SharedGameHistoryDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("GameHistory")));

//builder.Services.AddScoped<SharedGameConfigDbContext>(provider => provider.GetService<WheelConfigDbContext>()!);
builder.Services.AddScoped<SharedGameConfigDbContext, WheelConfigDbContext>();

var prizeGroupTypes = new List<Type> { typeof(WheelPrizeGroup), typeof(JackpotPrizeGroup) };

builder.Services.AddSingleton(prizeGroupTypes);

builder.Services.Resolve(builder.Configuration, prizeGroupTypes);

var app = builder.Build();

if (IsRunningInDocker())
{
    ConfigureConsulLifetime();
}

// Create Database
using var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
var context = serviceScope.ServiceProvider.GetService<WheelConfigDbContext>();
context!.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureConsulLifetime()
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var consulClient = app.Services.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration()
        {
            ID = Guid.NewGuid().ToString(),
            Name = "wheelapi",
            Address = "wheelapi",
            Port = 8080
        };

        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();
    });

    app.Lifetime.ApplicationStopped.Register(() =>
    {
        var consulClient = app.Services.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration()
        {
            ID = Guid.NewGuid().ToString(),
            Name = "wheelapi",
            Address = "wheelapi",
            Port = 8080
        };

        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
    });
}

void ConfigureConsul()
{
    builder.Services.Configure<ConsulConfig>(builder.Configuration.GetSection("Consul"));
    builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
    {
        var address = builder.Configuration["Consul:Host"];
        consulConfig.Address = new Uri(address);
    }));

    builder.Services.AddHostedService<ConsulHostedService>();
}

bool IsRunningInDocker()
{
    var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
    return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
}