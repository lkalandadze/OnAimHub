namespace GameLib.Application.Services.Abstract;

public interface IConsulGameService
{
    Task UpdateMetadataAsync<T>(Func<T> getDataFunc, string serviceId, string serviceName, int port, string[] tags);
}