using Consul;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;

namespace ApiGateway.Consul;

public class ConsulServiceWatcher : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConsulServiceWatcher> _logger;
    private readonly IFileConfigurationRepository _fileConfigurationRepository;
    private readonly ConsulConfig _consulConfig;
    private readonly string _ocelotJsonPath;

    public ConsulServiceWatcher(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulServiceWatcher> logger, IFileConfigurationRepository fileConfigurationRepository, IOptions<ConsulConfig> consulConfig)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
        _fileConfigurationRepository = fileConfigurationRepository;
        _consulConfig = consulConfig.Value;
        _ocelotJsonPath = Path.Combine(AppContext.BaseDirectory, "ocelot.json");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var services = await _consulClient.Agent.Services();

                // Ocelot config of getting routes
                var routes = services.Response.Values.Select(service => new Route
                {
                    //TODO ON MULTIPLE
                    DownstreamPathTemplate = $"/{service.Service}/{{everything}}",
                    DownstreamScheme = "http",
                    DownstreamHostAndPorts = new List<HostAndPort>
                    {
                        new HostAndPort { Host = service.Address, Port = service.Port }
                    },
                    UpstreamPathTemplate = $"/{service.Service}/{{everything}}",
                    UpstreamHttpMethod = new List<string> { "Get", "Post", "Put", "Delete" }
                }).ToList();

                var globalConfiguration = new GlobalConfigurations
                {
                    BaseUrl = _configuration["GlobalConfiguration:BaseUrl"],
                    ServiceDiscoveryProvider = new ServiceDiscoveryProvider
                    {
                        Type = "Consul",
                        Scheme = "http",
                        Host = "consul",
                        Port = 8500
                    }
                };

                var ocelotConfig = new OcelotConfiguration
                {
                    Routes = routes,
                    GlobalConfiguration = globalConfiguration
                };

                // Serialize the configuration to JSON
                var jsonConfig = JsonConvert.SerializeObject(ocelotConfig, Formatting.Indented);

                // Write the JSON to the ocelot.json file
                await File.WriteAllTextAsync(_ocelotJsonPath, jsonConfig, stoppingToken);

                _logger.LogInformation("Ocelot configuration updated and written to ocelot.json.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Ocelot configuration from Consul.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private FileConfiguration ConvertToOcelotFileConfiguration(OcelotConfiguration ocelotConfig)
    {
        return new FileConfiguration
        {
            GlobalConfiguration = new FileGlobalConfiguration
            {
                BaseUrl = ocelotConfig.GlobalConfiguration.BaseUrl,
                ServiceDiscoveryProvider = new FileServiceDiscoveryProvider
                {
                    Host = ocelotConfig.GlobalConfiguration.ServiceDiscoveryProvider.Host,
                    Port = ocelotConfig.GlobalConfiguration.ServiceDiscoveryProvider.Port,
                    Type = ocelotConfig.GlobalConfiguration.ServiceDiscoveryProvider.Type,
                    Scheme = ocelotConfig.GlobalConfiguration.ServiceDiscoveryProvider.Scheme
                }
            },
            Routes = ocelotConfig.Routes.Select(route => new FileRoute
            {
                DownstreamPathTemplate = route.DownstreamPathTemplate,
                DownstreamScheme = route.DownstreamScheme,
                DownstreamHostAndPorts = route.DownstreamHostAndPorts.Select(hp => new FileHostAndPort
                {
                    Host = hp.Host,
                    Port = hp.Port
                }).ToList(),
                UpstreamPathTemplate = route.UpstreamPathTemplate,
                UpstreamHttpMethod = route.UpstreamHttpMethod
            }).ToList()
        };
    }

    public class Route
    {
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<HostAndPort> DownstreamHostAndPorts { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
    }

    public class HostAndPort
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class GlobalConfigurations
    {
        public string BaseUrl { get; set; }
        public ServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }
    }

    public class ServiceDiscoveryProvider
    {
        public string Type { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class OcelotConfiguration
    {
        public List<Route> Routes { get; set; }
        public GlobalConfigurations GlobalConfiguration { get; set; }
    }
}