using Microsoft.Extensions.Options;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Middleware;
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
//builder.Services.AddTransient<IRequestHandler<LoginUserCommand, AuthResultDto>, LoginUserCommandHandler>();
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

builder.Services.Configure<LeaderBoardApiClientOptions>(
    builder.Configuration.GetSection("LeaderBoardApiClientOptions")
);

builder.Services.AddHttpClient<ILeaderBoardApiClient, LeaderboardApiClient>(
            (client, sp) =>
            {
                var catalogApiOptions = sp.GetRequiredService<IOptions<LeaderBoardApiClientOptions>>();
                var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();

                var baseAddress = catalogApiOptions.Value.BaseApiAddress;
                client.BaseAddress = new Uri(baseAddress);
                return new LeaderboardApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
            }
        );

builder.Services.Configure<AggregationClientOptions>(
builder.Configuration.GetSection("AggregationClientOptions")
);
builder.Services.AddHttpClient<IAggregationClient, AggregationClient>(
    (client, sp) =>
    {
        var catalogApiOptions = sp.GetRequiredService<IOptions<AggregationClientOptions>>();
        var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();
        catalogApiOptions.Value.NotBeNull();

        var baseAddress = catalogApiOptions.Value.BaseApiAddress;
        client.BaseAddress = new Uri(baseAddress);
        return new AggregationClient(client, catalogApiOptions, policyOptions);
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
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CoinModelJsonConverter());
    });

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{

}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }

    if (context.Request.Path == "/swagger")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }

    await next();
});

app.ApplyMigrations();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PermissionMiddleware>();
app.UseMiddleware<RequestHandlerMiddleware>();

app.MapControllers();

app.Run();