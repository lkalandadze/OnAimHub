using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using System.Collections.Concurrent;

namespace Hub.Application.Services.Concrete;

public class ActiveGameService : IActiveGameService
{
    private readonly ConcurrentDictionary<string, ActiveGameModel> _activeGames;

    public ActiveGameService()
    {
        _activeGames = new ConcurrentDictionary<string, ActiveGameModel>();
    }

    public void AddOrUpdateActiveGame(ActiveGameModel gameStatus)
    {
        _activeGames.AddOrUpdate(gameStatus.Id.ToString(), gameStatus, (key, existingValue) => gameStatus);
    }

    public bool RemoveActiveGame(string gameId)
    {
        return _activeGames.TryRemove(gameId, out _);
    }

    public IEnumerable<ActiveGameModel> GetActiveGames()
    {
        return _activeGames.Values;
    }
}