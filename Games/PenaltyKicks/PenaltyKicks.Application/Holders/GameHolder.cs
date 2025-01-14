using Microsoft.Extensions.DependencyInjection;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;

namespace PenaltyKicks.Application.Holders;

public class GameHolder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    internal static Dictionary<int, PenaltyGame> Games = [];

    public GameHolder(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;

        SetGenerators();
    }

    public bool HasActiveGame(int playerId)
    {
        return Games.ContainsKey(playerId);
    }

    public PenaltyGame? GetGameByPlayerId(int playerId)
    {
        if (!Games.ContainsKey(playerId))
        {
            return null;
        }

        return Games[playerId];
    }

    public async Task UpdateGameAsync(PenaltyGame game)
    {
        if (!Games.ContainsKey(game.PlayerId))
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation,
                $"Player with the specified ID: [{game.PlayerId}] does not exist in the active games.");
        }

        Games[game.PlayerId] = game;

        using var scope = _serviceScopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IPenaltyGameRepository>();
        gameRepository.Update(game);
        await gameRepository.SaveAsync();
    }

    private void SetGenerators()
    {
        var gameRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IPenaltyGameRepository>();
        var games = gameRepository.Query(g => !g.IsFinished);

        Games = games.ToDictionary(game => game.PlayerId, game => game);
    }
}