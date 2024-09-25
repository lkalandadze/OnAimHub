using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Concrete;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlayerService(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreatePlayersIfNotExist(IEnumerable<int> playerIds)
    {
        var missingPlayerIds = await _playerRepository.GetMissingPlayerIdsAsync(playerIds);

        foreach (var playerId in missingPlayerIds)
        {
            var player = new Player(playerId);

            await _playerRepository.InsertAsync(player);
            await _unitOfWork.SaveAsync();
        }
    }
}