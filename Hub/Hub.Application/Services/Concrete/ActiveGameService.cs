using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using Shared.Application.Models.Consul;
using System.Collections.Concurrent;

namespace Hub.Application.Services.Concrete;

public class ActiveGameService : IActiveGameService
{
    private readonly ConcurrentDictionary<string, GameRegisterResponseModel> _activeGames;

    public ActiveGameService()
    {
        _activeGames = new ConcurrentDictionary<string, GameRegisterResponseModel>();
    }

    public void AddOrUpdateActiveGame(GameRegisterResponseModel gameStatus)
    {
        _activeGames.AddOrUpdate(gameStatus.GameVersionId.ToString(), gameStatus, (key, existingValue) => gameStatus);
    }

    public bool RemoveActiveGame(string gameId)
    {
        return _activeGames.TryRemove(gameId, out _);
    }

    public IEnumerable<GameRegisterResponseModel> GetActiveGames()
    {
        return _activeGames.Values;
    }
}