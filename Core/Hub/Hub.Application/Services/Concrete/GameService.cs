﻿using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using System.Collections.Concurrent;

namespace Hub.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly ConcurrentDictionary<string, GameModel> _games = [];
    
    public void AddOrUpdateGame(GameModel gameStatus)
    {
        _games.AddOrUpdate(gameStatus.Address, gameStatus, (key, existingValue) => gameStatus);
    }

    public bool RemoveGame(string gameId)
    {
        return _games.TryRemove(gameId, out _);
    }

    public IEnumerable<GameModel> GetGames()
    {
        return _games.Values;
    }
}