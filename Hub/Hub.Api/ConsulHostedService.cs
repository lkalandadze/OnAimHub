using Consul;
using Microsoft.Extensions.Options;

namespace Hub.Api;

public class ConsulHostedService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<ConsulHostedService> _logger;
    private readonly string _serviceId;
    private readonly ConsulConfig _consulConfig;

    public ConsulHostedService(IConsulClient consulClient, IHostApplicationLifetime lifetime, ILogger<ConsulHostedService> logger, IOptions<ConsulConfig> consulConfig)
    {
        _consulClient = consulClient;
        _lifetime = lifetime;
        _logger = logger;
        _serviceId = Guid.NewGuid().ToString();
        _consulConfig = consulConfig.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var uri = new Uri(_consulConfig.ServiceAddress);

        var registration = new AgentServiceRegistration()
        {
            ID = _serviceId,
            Name = _consulConfig.ServiceName,
            Address = uri.Host,
            Port = _consulConfig.ServicePort,
            Tags = new[] { "hubapi" }
        };

        _logger.LogInformation("Registering with Consul");
        await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

        _lifetime.ApplicationStopping.Register(async () =>
        {
            _logger.LogInformation("Deregistering from Consul");
            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
