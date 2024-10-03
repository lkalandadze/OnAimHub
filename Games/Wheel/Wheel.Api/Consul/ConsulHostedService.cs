using Consul;
using Microsoft.Extensions.Options;

namespace Wheel.Api.Consul;

public class ConsulHostedService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<ConsulHostedService> _logger;
    private readonly string _serviceId;
    private readonly ConsulConfig _consulConfig;
    private readonly bool _registerWithConsul;

    public ConsulHostedService(IHostApplicationLifetime lifetime, ILogger<ConsulHostedService> logger, IOptions<ConsulConfig> consulConfig)
    {
        _lifetime = lifetime;
        _logger = logger;
        _serviceId = Guid.NewGuid().ToString();
        _consulConfig = consulConfig.Value;
        _registerWithConsul = bool.TryParse(Environment.GetEnvironmentVariable("REGISTER_WITH_CONSUL"), out bool result) && result;
        if (_registerWithConsul)
        {
            _consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(Environment.GetEnvironmentVariable("CONSUL_ADDRESS"));
            });
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_registerWithConsul)
        {
            _logger.LogInformation("Consul registration is disabled.");
            return;
        }

        _logger.LogInformation("Consul service address: {ServiceAddress}", _consulConfig.ServiceAddress);
        _logger.LogInformation("Consul service port: {ServicePort}", _consulConfig.ServicePort);

        try
        {
            var uri = new Uri(_consulConfig.ServiceAddress);

            var registration = new AgentServiceRegistration()
            {
                ID = _serviceId,
                Name = _consulConfig.ServiceName,
                Address = uri.Host,
                Port = _consulConfig.ServicePort,
                Tags = new[] { "WheelApi" }
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering with Consul");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_registerWithConsul)
        {
            _logger.LogInformation("Deregistering from Consul");
            return _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
        }

        return Task.CompletedTask;
    }
}
