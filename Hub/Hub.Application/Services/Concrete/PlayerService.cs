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
        //TODO: optimizing player getting
        foreach (var playerId in playerIds)
        {
            var player = await _playerRepository.OfIdAsync(playerId);

            if (player == null)
            {
                player = new Player(playerId);

                await _playerRepository.InsertAsync(player);
            }

            await _unitOfWork.SaveAsync();
        }
    }

    public async Task CreatePlayersIfNotExist(IEnumerable<Player> players)
    {
        await CreatePlayersIfNotExist(players.Select(x => x.Id));
    }
}