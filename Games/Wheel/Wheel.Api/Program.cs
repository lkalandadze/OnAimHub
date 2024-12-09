using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;
using GameLib.Infrastructure;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json;
using Wheel.Api;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

CreateDatabaseIfNotExists();
var host = CreateHostBuilder(args).Build();

#region Configuration Tests

var c = host.Services.CreateScope().ServiceProvider.GetRequiredService<WheelConfigDbContext>();

//Clear Configuration Tree
//c.Database.EnsureCreated();
//c.Prices.RemoveRange(c.Prices.ToList());
//c.WheelPrizes.RemoveRange(c.WheelPrizes.ToList());
//c.Rounds.RemoveRange(c.Rounds.ToList());
//c.GameConfigurations.RemoveRange(c.GameConfigurations.ToList());
//c.SaveChanges();

//Create New Configuration
//var prizeType = new PrizeType("Prize Type 2", true, "OnAimCoin");

//var newConfig = new WheelConfiguration()
//{
//    Name = "Wheel Configuration 1",
//    Value = 1000,
//    Prices = new List<Price>
//    {
//        new Price() { Id = Random.Shared.Next(1, 1000).ToString(), Value = 1, Multiplier = 10, CurrencyId = "OnAimCoin" },
//    },
//    Rounds = new List<Round>
//    {
//        new Round()
//        {
//            Name = "Round 1",
//            NextPrizeIndex = 1,
//            Sequence = [ 1, 2, 3, 4, 5 ],
//            Prizes = new List<WheelPrize>
//            {
//                new WheelPrize() { Name = "Prize A", PrizeType = prizeType, Value = 1, Probability = 10, WheelIndex= 1 },
//                new WheelPrize() { Name = "Prize B", PrizeType = prizeType, Probability = 20, WheelIndex= 2 },
//                new WheelPrize() { Name = "Prize C", PrizeType = prizeType, Probability = 30, WheelIndex= 3 },
//            }
//        },
//        new Round()
//        {
//            Name = "Round 2",
//            NextPrizeIndex = 2,
//            Sequence = [ 21, 22, 23 ],
//            Prizes = new List<WheelPrize>
//            {
//                new WheelPrize() { Name = "Prize D", PrizeType = prizeType, Value = 21, Probability = 210, WheelIndex= 21 },
//                new WheelPrize() { Name = "Prize F", PrizeType = prizeType, Value = 22, Probability = 220, WheelIndex= 22 },
//                new WheelPrize() { Name = "Prize G", PrizeType = prizeType, Value = 23, Probability = 230, WheelIndex= 23 },
//            }
//        },
//    },
//    IsActive = true,
//    CorrelationId = Guid.NewGuid(),
//    FromTemplateId = 999,
//};

//string newConfigJson = JsonConvert.SerializeObject(newConfig);
//await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.CreateAsync(newConfigJson));

//c.SaveChanges();

////Get Configuration
//var existingConfig = await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.GetByIdAsync(c.GameConfigurations.First().Id)) as WheelConfiguration;

////Update Configuration
//if (existingConfig != null)
//{
//    existingConfig.Name = DateTime.Now.ToString();

//    existingConfig.Rounds.Last().Prizes.First().Probability = 321;

//    //existingConfig.Rounds.Remove(existingConfig.Rounds.First());
//    //existingConfig.Rounds.First().Prizes.Remove(existingConfig.Rounds.First().Prizes.First());

//    existingConfig.Rounds.First().Prizes.First().Name += " UPDATED";
//    existingConfig.Rounds.Last().Prizes.First().Name += " UPDATED";
//    existingConfig.Rounds.Last().Prizes.First().Probability = 999;

//    existingConfig.Rounds.Add(new Round()
//    {
//        Id = 0,
//        NextPrizeIndex = 0,
//        Prizes = new List<WheelPrize>() { new WheelPrize(DateTime.Now.ToString()) { Value = 123, Probability = 10, PrizeTypeId = 1 } },
//        Sequence = [1, 2, 3],
//        Name = "RMMRMRMRM",
//    });
//}

//var configJson = JsonConvert.SerializeObject(existingConfig, new JsonSerializerSettings
//{
//    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//});

//await host.Services.ExecuteAsync<IGameConfigurationService>(async x => await x.UpdateAsync(existingConfig));

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
    dbContext.Database.EnsureCreated();

    _ = new DbInitializer(dbContext);
}