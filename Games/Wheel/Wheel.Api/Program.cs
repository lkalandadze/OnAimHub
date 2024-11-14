using GameLib.Application.Services.Abstract;
using GameLib.Infrastructure;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wheel.Api;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

CreateDatabaseIfNotExists();
var host = CreateHostBuilder(args).Build();

#region Configuration Tests

//var c = host.Services.CreateScope().ServiceProvider.GetRequiredService<WheelConfigDbContext>();

////Clear Configuration Tree
//c.Database.EnsureCreated();
//var s = c.Segments.ToList();
//c.Segments.RemoveRange(s);
//c.Prices.RemoveRange(c.Prices.ToList());
//c.WheelPrizes.RemoveRange(c.WheelPrizes.ToList());
//c.Rounds.RemoveRange(c.Rounds.ToList());
//c.GameConfigurations.RemoveRange(c.GameConfigurations.ToList());
//c.SaveChanges();

////Create New Configuration
//var newConfigJson = "{ \"Name\": \"Hardcoded Wheel Configuration\", \"Value\": 1000, \"IsActive\": true, \"Rounds\": [ { \"Name\": \"Hardcoded Round 1\", \"Sequence\": [1, 2, 3], \"Prizes\": [ { \"Name\": \"Prize A\", \"Value\": 100, \"Probability\": 50, \"PrizeTypeId\": 1, \"WheelIndex\": 0 }, { \"Name\": \"Prize B\", \"Value\": 200, \"Probability\": 30, \"PrizeTypeId\": 2, \"WheelIndex\": 1 } ] }, { \"Name\": \"Hardcoded Round 2\", \"Sequence\": [1, 2, 3], \"Prizes\": [ { \"Name\": \"Prize C\", \"Value\": 150, \"Probability\": 40, \"PrizeTypeId\": 1, \"WheelIndex\": 2 }, { \"Name\": \"Prize D\", \"Value\": 250, \"Probability\": 20, \"PrizeTypeId\": 3, \"WheelIndex\": 3 } ] } ], \"Prices\": [ { \"Id\": \"Price5\", \"Value\": 200.50, \"Multiplier\": 1.5, \"CurrencyId\": \"OnAimCoin\" }, { \"Id\": \"Price6\", \"Value\": 350.75, \"Multiplier\": 2.0, \"CurrencyId\": \"OnAimCoin\" } ], \"Segments\": [ { \"Id\": \"Segment5\", \"IsDeleted\": true }, { \"Id\": \"Segment6\", \"IsDeleted\": false } ] }";
//await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.CreateAsync(newConfigJson));
//c.SaveChanges();

////Get Configuration
//var config = await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.GetByIdAsync(c.GameConfigurations.First().Id)) as WheelConfiguration;

////Update Configuration
//if (config != null)
//{
//    config.Name = DateTime.Now.ToString();

//    config.Rounds.Last().Prizes.First().Probability = 321;

//    //config.Rounds.Remove(config.Rounds.First());
//    //config.Rounds.First().Prizes.Remove(config.Rounds.First().Prizes.First());

//    config.Rounds.First().Prizes.First().Name += " UPDATED";
//    config.Rounds.Last().Prizes.First().Name += " UPDATED";
//    config.Rounds.Last().Prizes.First().Probability = 999;

//    config.Rounds.Add(new Round()
//    {
//        Id = 0,
//        NextPrizeIndex = 0,
//        Prizes = new List<WheelPrize>() { new WheelPrize(DateTime.Now.ToString()) { Value = 123, Probability = 10, PrizeTypeId = 1 } },
//        Sequence = [1, 2, 3],
//        Name = "RMMRMRMRM",
//    });
//}

//var configJson = JsonConvert.SerializeObject(config, new JsonSerializerSettings
//{
//    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//});

//await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.UpdateAsync(configJson));

#endregion

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
    dbContext.Database.Migrate();

    _ = new DbInitializer(dbContext);
}