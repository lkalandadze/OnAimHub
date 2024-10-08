using GameLib.Infrastructure;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Wheel.Api;
using Wheel.Infrastructure.DataAccess;

CreateDatabaseIfNotExists();
var host = CreateHostBuilder(args).Build();
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

