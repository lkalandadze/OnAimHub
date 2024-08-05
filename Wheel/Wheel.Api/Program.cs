using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess;
using Wheel.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WheelConfigDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("GameConfig")));

builder.Services.AddDbContext<SharedGameHistoryDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("GameHistory")));

var app = builder.Build();

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
