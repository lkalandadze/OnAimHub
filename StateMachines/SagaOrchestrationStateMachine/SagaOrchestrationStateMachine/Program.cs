using Hub.Application.Converters;
using Hub.Application.Models.Coin;
using SagaOrchestrationStateMachine;
using Shared.Lib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<HubService>(sp =>
new HubService("https://localhost:7069", new HttpClient()));
builder.Services.AddSingleton<LeaderBoardService>(sp =>
new LeaderBoardService("https://localhost:7041", new HttpClient()));
//builder.Services.AddSingleton<WheelService>(sp =>
//new WheelService("http://192.168.10.42:8005", new HttpClient()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
