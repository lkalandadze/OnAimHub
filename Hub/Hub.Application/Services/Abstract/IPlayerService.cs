using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface IPlayerService
{
    Task CreatePlayersIfNotExist(IEnumerable<int> playerIds);

    Task CreatePlayersIfNotExist(IEnumerable<Player> players);
}