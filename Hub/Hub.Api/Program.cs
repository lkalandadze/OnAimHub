using Hub.Infrastructure.DataAccess;
using Serilog;

var host = CreateHostBuilder(args).Build();
CreateDatabaseIfNotExists(host);
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

static void CreateDatabaseIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<HubDbContext>();
        dbContext.Database.EnsureCreated();
    }
}