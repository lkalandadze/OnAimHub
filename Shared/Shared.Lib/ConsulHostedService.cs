using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Lib;

public class ConsulHostedService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConsulHostedService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private string _registrationId;
    private bool _isDeregistered;

    public ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulHostedService> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        _consulClient = consulClient;
        _configuration = configuration;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _registrationId = _configuration["Consul:ServiceId"];

        var registration = new AgentServiceRegistration()
        {
            ID = _registrationId,
            Name = _configuration["Consul:ServiceName"],
            Address = _configuration["Consul:ServiceAddress"],
            Port = int.Parse(_configuration["Consul:ServicePort"])
        };

        _logger.LogInformation("Registering service with Consul");
        _hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            _consulClient.Agent.ServiceRegister(registration, cancellationToken).Wait();
        });

        _hostApplicationLifetime.ApplicationStopping.Register(() =>
        {
            if (!_isDeregistered)
            {
                DeregisterService();
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_isDeregistered)
        {
            DeregisterService();
        }
        return Task.CompletedTask;
    }

    private void DeregisterService()
    {
        if (!string.IsNullOrEmpty(_registrationId))
        {
            _logger.LogInformation("Deregistering service from Consul");
            _consulClient.Agent.ServiceDeregister(_registrationId).Wait();
            _isDeregistered = true;
        }
    }
}