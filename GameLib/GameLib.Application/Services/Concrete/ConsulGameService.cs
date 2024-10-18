using Consul;
using GameLib.Application.Services.Abstract;
using System.Text.Json;

namespace GameLib.Application.Services.Concrete;

public class ConsulGameService : IConsulGameService
{
    private readonly IConsulClient? _consulClient;

    public ConsulGameService(IConsulClient? consulClient)
    {
        _consulClient = consulClient;
    }

    public async Task UpdateMetadataAsync<T>(Func<T> getDataFunc, string serviceId, string serviceName, int port, string[] tags)
    {
        var data = getDataFunc();
        var serializedData = JsonSerializer.Serialize(data);

        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = serviceName,
            Address = serviceName,
            Port = port,
            Tags = tags,
            Meta = new Dictionary<string, string>()
            {
                { "GameData", serializedData }
            }
        };

        await _consulClient.Agent.ServiceRegister(registration);
    }
}