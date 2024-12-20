using Microsoft.Extensions.Options;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Extensions;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Infrasturcture;
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
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<HttpClientService>();
builder.Services.Configure<HubApiClientOptions>(
builder.Configuration.GetSection("HubApiClientOptions")
);
builder.Services.AddHttpClient<IHubApiClient, HubApiClient>(
    (client, sp) =>
    {
        var catalogApiOptions = sp.GetRequiredService<IOptions<HubApiClientOptions>>();
        var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();
        catalogApiOptions.Value.NotBeNull();

        var baseAddress = catalogApiOptions.Value.BaseApiAddress;
        client.BaseAddress = new Uri(baseAddress);
        return new HubApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
    }
);


builder.Services.Configure<SagaApiClientOptions>(
builder.Configuration.GetSection("SagaApiClientOptions")
);
builder.Services.AddHttpClient<ISagaApiClient, SagaApiClient>(
    (client, sp) =>
    {
        var catalogApiOptions = sp.GetRequiredService<IOptions<SagaApiClientOptions>>();
        var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();
        catalogApiOptions.Value.NotBeNull();

        var baseAddress = catalogApiOptions.Value.BaseApiAddress;
        client.BaseAddress = new Uri(baseAddress);
        return new SagaApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
    }
);

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

builder.Services.AddControllersWithViews()
    //.AddNewtonsoftJson(options =>
    //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CoinModelJsonConverter());
    });

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