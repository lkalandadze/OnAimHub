using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Abstract;

public interface IPlayerLogService
{
    Task CreatePlayerLogAsync(string logMessage, int playerId, PlayerLogType playerLogType);
}