using GameLib.Application.Configurations;
using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PenaltyKicks.Application.Holders;
using PenaltyKicks.Application.Models.PenaltyKicks;
using PenaltyKicks.Application.Services.Abstract;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using PenaltyKicks.Domain.Enums;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace PenaltyKicks.Application.Services.Concrete;

public class PenaltyService : IPenaltyService
{
    private readonly IPriceRepository _priceRepository;
    private readonly IPenaltyGameRepository _gameRepository;
    private readonly IHubService _hubService;
    private readonly IAuthService _authService;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly GameHolder _gameHolder;
    private readonly GameInfoConfiguration _gameInfoConfig;

    public PenaltyService(
        IPriceRepository priceRepository,
        IPenaltyGameRepository gameRepository,
        IHubService hubService,
        IAuthService authService,
        ConfigurationHolder configurationHolder,
        GameHolder gameHolder,
        IOptions<GameInfoConfiguration> gameInfoConfig)
    {
        _priceRepository = priceRepository;
        _gameRepository = gameRepository;
        _hubService = hubService;
        _authService = authService;
        _configurationHolder = configurationHolder;
        _gameHolder = gameHolder;
        _gameInfoConfig = gameInfoConfig.Value;
    }

    public InitialDataResponseModel GetInitialDataAsync(int promotionId)
    {
        var playerId = _authService.GetCurrentPlayerId();
        var configuration = _configurationHolder.GetConfiguration<PenaltyConfiguration>(promotionId);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Configuration with the specified promotion ID: [{promotionId}] was not found.");
        }

        var kicksCount = configuration.KicksCount;
        var hasActiveGame = _gameHolder.HasActiveGame(playerId);
        var game = _gameHolder.GetGameByPlayerId(playerId);

        return new InitialDataResponseModel
        {
            HasActiveGame = hasActiveGame,
            GameConfigInfo = new GameConfigInitialDataResponseModel
            {
                BetPrices = _configurationHolder.GetPrices(promotionId).OrderBy(p => p.BetAmount),
                KicksCount = kicksCount
            },
            ActiveGameInfo = !hasActiveGame ? null : new ActiveGameInfoInitialDataResponseModel
            {
                CurrentKickIndex = game!.CurrentKickIndex,
                GoalsScored = game.GoalsScored,
                KicksRemaining = game.KickSequence.Count - game.CurrentKickIndex,
                BetPriceId = game.BetPriceId,
            },
        };
    }

    public async Task<BetResponseModel> PlaceBetAsync(int promotionId, int betPriceId)
    {
        var playerId = _authService.GetCurrentPlayerId();

        if (GameHolder.Games.TryGetValue(playerId, out var playerGame))
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation,
                $"Player with the specified ID: [{playerId}] is already in game [{playerGame.Id}].");
        }

        var configuration = _configurationHolder.GetConfiguration<PenaltyConfiguration>(promotionId);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Configuration with the specified promotion ID: [{promotionId}] was not found.");
        }

        var price = await _priceRepository.Query(p => p.Id == betPriceId).FirstOrDefaultAsync();

        if (price == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Price with the specified ID: [{betPriceId}] was not found.");
        }

        await _hubService.BetTransactionAsync(_gameInfoConfig.GameId, "PenaltyKick", promotionId, price.Value);

        var prizeGroup = _configurationHolder.GetPrizeGroups(promotionId).Cast<PenaltyPrizeGroup>().FirstOrDefault();
        var prize = GeneratorHolder.GetPrizeAsync<PenaltyPrize>(prizeGroup!.Id);

        if (prize == null)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "The prize generation failed. No prize was generated for the specified criteria. Please try again or contact support."
            );
        }

        var kicksCount = configuration!.KicksCount;
        var kicksSequence = GenerateKicksSequence(kicksCount, prize.Value);

        var game = new PenaltyGame(playerId, price.Id, prize.Id, prize.Value, price.Multiplier, prize.CoinId, kicksSequence);
        
        lock (GameHolder.Games)
        {
            GameHolder.Games.Add(playerId, game);
        }

        await _gameRepository.InsertAsync(game);
        await _gameRepository.SaveAsync();

        var selectedPrize = prize.Value > 0
            ? prize
            : _configurationHolder.GetPrizeGroups(promotionId)
                                  .First()
                                  .GetBasePrizes()
                                  .FirstOrDefault(p => p.Value > 0)!;

        return new BetResponseModel
        {
            PrizeId = selectedPrize.Id,
            PrizeValue = selectedPrize.Value,
            Coin = selectedPrize.CoinId.Split('_')[1],
        };
    }

    public async Task<KickResponseModel> PenaltyKick(int promotionId)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var configuration = _configurationHolder.GetConfiguration<PenaltyConfiguration>(promotionId);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Configuration with the specified promotion ID: [{promotionId}] was not found.");
        }

        var game = _gameHolder.GetGameByPlayerId(playerId);

        if (game == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation,
                $"Player with the specified ID: [{playerId}] has not started a game or the game session is not active.");
        }

        var kickResult = game.KickSequence[game.CurrentKickIndex];
        game.IncreaseCurrentKickIndex(1);

        if (kickResult)
        {
            game.IncreaseGoalsScored(1);
        }

        var totalKicks = game.KickSequence.Count;
        var requiredGoalsToWin = (totalKicks / 2) + 1;

        var gameState = GameState.InProgress;

        if (game.GoalsScored >= requiredGoalsToWin)
        {
            gameState = GameState.Won;
        }
        else if ((totalKicks - game.CurrentKickIndex + game.GoalsScored) < requiredGoalsToWin)
        {
            gameState = GameState.Lost;
        }

        await _gameHolder.UpdateGameAsync(game);

        var response = new KickResponseModel
        {
            IsGoal = kickResult,
            GameState = gameState.ToString(),
            GoalsScored = game.GoalsScored,
            KicksRemaining = totalKicks - game.CurrentKickIndex,
            RequiredGoalsToWin = requiredGoalsToWin
        };

        if (gameState != GameState.InProgress)
        {
            GameHolder.Games.Remove(playerId);

            game.FinishGame();
            _gameRepository.Update(game);
            await _gameRepository.SaveAsync();

            if (gameState == GameState.Won)
            {
                await _hubService.WinTransactionAsync(_gameInfoConfig.GameId, "PenaltyKick", game.CoinId, promotionId, game.PriceMultiplier * game.PrizeValue);
            }
        }

        return response;
    }

    private List<bool> GenerateKicksSequence(int kicksCount, int prizeValue)
    {
        var rand = new Random();
        var trueRatio = prizeValue > 0 ? 0.6 : 0.4;

        var kicksSequence = Enumerable.Range(0, kicksCount)
                                      .Select(_ => rand.NextDouble() < trueRatio)
                                      .ToList();

        if (prizeValue > 0 && kicksSequence.Count(k => k) <= kicksCount / 2)
        {
            kicksSequence[rand.Next(kicksCount)] = true; // Flip a random false to true.
        }
        else if (prizeValue <= 0 && kicksSequence.Count(k => !k) <= kicksCount / 2)
        {
            kicksSequence[rand.Next(kicksCount)] = false; // Flip a random true to false.
        }

        if (kicksCount % 2 == 0 && kicksSequence.Count(k => k) == kicksSequence.Count(k => !k))
        {
            kicksSequence.Add(prizeValue > 0);
        }

        return kicksSequence;
    }
}