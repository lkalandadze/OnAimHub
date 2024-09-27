using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IPlayerLogService
{
    Task CreatePlayerLogAsync(string logMessage, int playerId, PlayerLogType playerLogType);
}