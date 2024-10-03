using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Middleware;
using OnAim.Admin.APP;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using Serilog;
using Serilog.Events;
using OnAim.Admin.Infrasturcture.Repositories;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddControllers()
                .Services.AddCustomSwagger()
                .AddApp(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

builder.AddCustomHttpClients();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IConfigurationRepository<>), typeof(ConfigurationRepository<>));
builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
builder.Services.AddScoped(typeof(ILeaderBoardReadOnlyRepository<>), typeof(LeaderBoardReadOnlyRepository<>));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnectionString")!);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    app.ApplyMigrations();

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PermissionMiddleware>();
//app.UseMiddleware<RequestHandlerMiddleware>();

app.MapControllers();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
