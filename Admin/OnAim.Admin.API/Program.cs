using OnAim.Admin.API.Extensions;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Domain.HubEntities.Models;
using OnAim.Admin.Infrasturcture;
using Serilog;
using Serilog.Events;
using Shared.Lib.SwaggerFilters;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<HttpClientService>();

builder.Services
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddControllers()
                .Services.AddCustomSwagger()
                .AddApp(builder.Configuration, consumerAssemblyMarkerType: typeof(Program))
                .AddInfrastructure(builder.Configuration)
                ;
builder.AddCustomHttpClients();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new CoinModelJsonConverter());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<PolymorphismSchemaFilter<CreateCoinModel>>();
});
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<PermissionMiddleware>();
app.UseMiddleware<RequestHandlerMiddleware>();

app.MapControllers();

app.Run();