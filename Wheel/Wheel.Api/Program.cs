using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess;
using Shared.ServiceRegistry;
using Wheel.Infrastructure.DataAccess;
using Wheel.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
