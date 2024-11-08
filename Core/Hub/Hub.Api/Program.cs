using Hub.Infrastructure;
using Hub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;

CreateDatabaseIfNotExists();
var host = CreateHostBuilder(args).Build();
host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders(); // Clears default providers
            logging.AddSerilog(); // Adds Serilog as the logging provider
        })
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

    string connectionString = configuration.GetConnectionString("OnAimHub")!;

    var optionsBuilder = new DbContextOptionsBuilder<HubDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    var dbContext = new HubDbContext(optionsBuilder.Options);
    dbContext.Database.EnsureCreated();

    _ = new DbInitializer(dbContext);
}