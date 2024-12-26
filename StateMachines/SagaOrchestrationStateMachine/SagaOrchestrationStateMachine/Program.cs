using Hub.Application.Converters;
using Hub.Application.Models.Coin;
using Microsoft.Extensions.Options;
using SagaOrchestrationStateMachine;
using SagaOrchestrationStateMachine.Services;
using Shared.Lib.SwaggerFilters;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<LeaderBoardService>(sp =>
new LeaderBoardService("http://192.168.10.42:8002", new HttpClient()));
builder.Services.AddSingleton<WheelService>(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri("http://192.168.10.42:8005")
    };

    var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"a:a"));
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

    return new WheelService("http://192.168.10.42:8005", httpClient);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.Configure<HubApiClientOptions>(
    builder.Configuration.GetSection("HubApiClientOptions")
);
builder.Services.AddHttpClient<IHubApiClient, HubApiClient>(
            (client, sp) =>
            {
                var catalogApiOptions = sp.GetRequiredService<IOptions<HubApiClientOptions>>();
                var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();

                var baseAddress = catalogApiOptions.Value.BaseApiAddress;
                client.BaseAddress = new Uri(baseAddress);
                return new HubApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
            }
        );

builder.Services.Configure<WheelApiClientOptions>(
    builder.Configuration.GetSection("WheelApiClientOptions")
);
builder.Services.AddHttpClient<IWheelApiClientApiClient, WheelApiClient>(
            (client, sp) =>
            {
                var catalogApiOptions = sp.GetRequiredService<IOptions<WheelApiClientOptions>>();
                var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();

                var baseAddress = catalogApiOptions.Value.BaseApiAddress;
                client.BaseAddress = new Uri(baseAddress);
                return new WheelApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
            }
        );

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new CoinModelJsonConverter());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<PolymorphismSchemaFilter<CreateCoinModel>>();
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
