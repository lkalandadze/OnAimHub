using GameLib.Application.Services.Abstract;
using GameLib.Infrastructure;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Wheel.Api;
using Wheel.Infrastructure.DataAccess;

CreateDatabaseIfNotExists();
var host = CreateHostBuilder(args).Build();

//var i = host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.GetByIdAsync(27)).Result;

var json = "{ \"Id\": 29, \"Name\": \"aaaaaaaa\", \"Value\": 1000, \"IsActive\": true, \"Rounds\": [ { \"Id\": 37, \"Name\": \"aaaaaaaaaaa 1\", \"Sequence\": [1, 2, 3, 4], \"Prizes\": [ { \"Id\": 69, \"Name\": \"Prize A\", \"Value\": 5555, \"Probability\": 1221, \"PrizeTypeId\": 1, \"WheelIndex\": 0 }, { \"Id\": 70, \"Name\": \"Prize B\", \"Value\": 200, \"Probability\": 59, \"PrizeTypeId\": 2, \"WheelIndex\": 1 } ] }, { \"Id\": 38, \"Name\": \"aaaaaaaaaaa 2\", \"Sequence\": [1, 2, 3], \"Prizes\": [ { \"Id\": 71, \"Name\": \"Prize C\", \"Value\": 150, \"Probability\": 40, \"PrizeTypeId\": 1, \"WheelIndex\": 2 }, { \"Id\": 72, \"Name\": \"Prize D\", \"Value\": 250, \"Probability\": 20, \"PrizeTypeId\": 3, \"WheelIndex\": 3 } ] } ], \"Prices\": [ { \"Id\": \"Price5\", \"Value\": 55555, \"Multiplier\": 1.5, \"CurrencyId\": \"OnAimCoin\" }, { \"Id\": \"Price6\", \"Value\": 55555, \"Multiplier\": 2.0, \"CurrencyId\": \"OnAimCoin\" } ], \"Segments\": [ { \"Id\": \"Segment5\", \"IsDeleted\": false}, { \"Id\": \"Segment6\", \"IsDeleted\": true} ] }";

var j = host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.UpdateAsync(json));

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

static void CreateDatabaseIfNotExists()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    string connectionString = configuration.GetConnectionString("GameConfig")!;

    //var optionsBuilder = new DbContextOptionsBuilder<SharedGameConfigDbContext<WheelConfiguration>>();
    var optionsBuilder = new DbContextOptionsBuilder<SharedGameConfigDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    var dbContext = new WheelConfigDbContext(optionsBuilder.Options);
    dbContext.Database.EnsureCreated();

    _ = new DbInitializer(dbContext);
}