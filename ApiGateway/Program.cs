namespace ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.CaptureStartupErrors(true);
                webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                webBuilder.UseStartup<Startup>();
            });
}