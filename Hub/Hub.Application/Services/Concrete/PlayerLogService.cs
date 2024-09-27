using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Concrete;

public class PlayerLogService : IPlayerLogService
{
    private readonly IPlayerLogRepository _playerLogRepository;

    public PlayerLogService(IPlayerLogRepository playerLogRepository)
    {
        _playerLogRepository = playerLogRepository;
    }

    public async Task CreatePlayerLogAsync(string logMessage, int playerId, PlayerLogType playerLogType)
    {
        var log = new PlayerLog(logMessage, playerId, playerLogType);
        await _playerLogRepository.InsertAsync(log);
    }
}