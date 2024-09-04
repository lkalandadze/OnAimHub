using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Factory;
using OnAim.Admin.API.Middleware;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.Identity;
using OnAim.Admin.Identity.Entities;
using OnAim.Admin.Identity.Persistance;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Repository;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
                //.AddCustomIdentity()
                .AddCustomJwtAuthentication(builder.Configuration)
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddControllers()
                .Services.AddCustomSwagger();

builder.Services.AddIdentityServerAuthentication<ApplicationIdentityDbContext, User, ApplicationUserManager>(builder.Configuration);

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IConfigurationRepository<>), typeof(ConfigurationRepository<>));
builder.Services.AddScoped(ApplicationContextFactory.Create);
builder.Services.AddApp(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

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
    app.ApplyMigrations();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PermissionMiddleware>();
//app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
