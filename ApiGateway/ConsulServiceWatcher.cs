using Consul;
using System.Text.Json;

namespace ApiGateway;

public class ConsulServiceWatcher : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConsulServiceWatcher> _logger;
    private readonly string _ocelotConfigPath;

    public ConsulServiceWatcher(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulServiceWatcher> logger)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
        _ocelotConfigPath = "ocelot.json";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var services = await _consulClient.Agent.Services();
                var routes = services.Response.Values.Select(service => new
                {
                    DownstreamPathTemplate = $"/{service.Service}/{service.ID}",
                    DownstreamScheme = "http",
                    DownstreamHostAndPorts = new[]
                    {
                        new { Host = service.Address, Port = service.Port }
                    },
                    UpstreamPathTemplate = $"/{service.Service}/{service.ID}",
                    UpstreamHttpMethod = new[] { "Get", "Post", "Put", "Delete" }
                }).ToList();

                var ocelotConfig = new
                {
                    GlobalConfiguration = _configuration.GetSection("GlobalConfiguration").GetChildren().ToDictionary(x => x.Key, x => x.Value),
                    Routes = routes
                };

                await File.WriteAllTextAsync(_ocelotConfigPath, JsonSerializer.Serialize(ocelotConfig, new JsonSerializerOptions { WriteIndented = true }), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Ocelot configuration from Consul.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Adjust the delay as needed
        }
    }
}
