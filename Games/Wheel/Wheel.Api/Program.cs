using GameLib.Infrastructure;
using Wheel.Api;
using Wheel.Infrastructure.DataAccess;

var host = CreateHostBuilder(args).Build();
CreateDatabaseIfNotExists(host);
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

static void CreateDatabaseIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<WheelConfigDbContext>();
        dbContext.Database.EnsureCreated();

        _ = new DbInitializer(scope);
    }
}