using Consul;

namespace GameLib.Application.Services.Abstract;

public interface IConsulGameService
{
    Task UpdateMetadataAsync<T>(Func<List<T>> getDataFunc, string serviceId, string serviceName, int port, string[] tags);
}
