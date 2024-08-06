using Consul;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;

namespace ApiGateway;

public class ConsulServiceWatcher : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConsulServiceWatcher> _logger;
    private readonly IFileConfigurationRepository _fileConfigurationRepository;
    private readonly ConsulConfig _consulConfig;

    public ConsulServiceWatcher(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulServiceWatcher> logger, IFileConfigurationRepository fileConfigurationRepository, IOptions<ConsulConfig> consulConfig)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
        _fileConfigurationRepository = fileConfigurationRepository;
        _consulConfig = consulConfig.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var services = await _consulClient.Agent.Services();

                //Ocelot config of getting routes
                var routes = services.Response.Values.Select(service => new FileRoute
                {
                    DownstreamPathTemplate = $"/{service.Service}/{{everything}}",
                    DownstreamScheme = "http",
                    DownstreamHostAndPorts = new List<FileHostAndPort>
                        {
                            new FileHostAndPort { Host = service.Address, Port = service.Port }
                        },
                    UpstreamPathTemplate = $"/{service.Service}/{{everything}}",
                    UpstreamHttpMethod = new List<string> { "Get", "Post", "Put", "Delete" }
                }).ToList();

                var ocelotConfig = new FileConfiguration
                {
                    GlobalConfiguration = new FileGlobalConfiguration
                    {
                        BaseUrl = _configuration["GlobalConfiguration:BaseUrl"],
                        ServiceDiscoveryProvider = new FileServiceDiscoveryProvider
                        {
                            Type = _consulConfig.Type,
                            Scheme = "http",
                            Host = _consulConfig.Host,
                            Port = _consulConfig.Port
                        }
                    },
                    Routes = routes
                };

                await _fileConfigurationRepository.Set(ocelotConfig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Ocelot configuration from Consul.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
