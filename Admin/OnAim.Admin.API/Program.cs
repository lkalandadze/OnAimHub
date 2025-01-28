using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Middleware;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Infrasturcture;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// Logging Configuration
// ----------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ----------------------------------------------------
// Service Registration
// ----------------------------------------------------
// Authentication
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

// Custom HTTP Clients
builder.Services.AddScoped<HttpClientService>();
builder.AddCustomHttpClients();

// Additional Services and Extensions
builder.Services
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices(builder.Configuration)
                .AddControllers()
                .Services.AddCustomSwagger()
                .AddApp(builder.Configuration, consumerAssemblyMarkerType: typeof(Program))
                .AddInfrastructure(builder.Configuration);

builder.AddCustomHttpClients();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CoinModelJsonConverter());
    });

var app = builder.Build();

// Logging Middleware
app.UseSerilogRequestLogging();

// Middleware for Redirecting to Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" || context.Request.Path == "/swagger")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});

// Apply Migrations
app.ApplyMigrations();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS
app.UseCors("MyPolicy");

// Enable HTTPS Redirection
app.UseHttpsRedirection();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Custom Middleware
// app.UseMiddleware<PermissionMiddleware>();
app.UseMiddleware<RequestHandlerMiddleware>();

// Map Controllers
app.MapControllers();

// ----------------------------------------------------
// Run the Application
// ----------------------------------------------------
app.Run();