using SagaOrchestrationStateMachine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<HubService>(sp =>
new HubService("https://localhost:7069", new HttpClient()));
builder.Services.AddSingleton<LeaderBoardService>(sp =>
new LeaderBoardService("http://192.168.10.42:8002", new HttpClient()));
//builder.Services.AddSingleton<WheelService>(sp =>
//new WheelService("http://192.168.10.42:8005", new HttpClient()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
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
