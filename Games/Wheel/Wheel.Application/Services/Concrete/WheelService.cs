using GameLib.Application;
using GameLib.Application.Configurations;
using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Models.Round;
using Wheel.Application.Models.WheelPrize;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class WheelService : IWheelService
{
    private readonly IPriceRepository _priceRepository;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IMessageBus _messageBus;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly GameSettings _gameSettings;
    private readonly GameInfoConfiguration _gameInfoConfig;

    public WheelService(
        IPriceRepository priceRepository,
        IAuthService authService,
        IHubService hubService,
        IMessageBus messageBus,
        ConfigurationHolder configurationHolder,
        GameSettings gameSettings,
        IOptions<GameInfoConfiguration> gameInfoConfig)
    {
        _priceRepository = priceRepository;
        _authService = authService;
        _hubService = hubService;
        _messageBus = messageBus;
        _configurationHolder = configurationHolder;
        _gameSettings = gameSettings;
        _gameInfoConfig = gameInfoConfig.Value;
    }

    public InitialDataResponseModel GetInitialData(int promotionId)
    {
        var mappedRounds = _configurationHolder.GetPrizeGroups(promotionId).Cast<Round>();

        var rounds = mappedRounds.Select(round => new RoundInitialData
        {
            Id = round.Id,
            Name = round.Name,
            Prizes = round.GetBasePrizes()
                .Cast<WheelPrize>()
                .OrderBy(prize => prize.WheelIndex) // Sort prizes by WheelIndex
                .Select(prize => new WheelPrizeInitialData
                {
                    Id = prize.Id,
                    Name = prize.Name,
                    Value = prize.Value,
                    WheelIndex = prize.WheelIndex,
                    Coin = prize.CoinId.Split('_')[1],
                })
            });

        return new InitialDataResponseModel()
        {
            Prices = _configurationHolder.GetPrices(promotionId),
            Rounds = rounds,
        };
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<WheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel request)
        where TPrize : BasePrize
    {
        //if (!_gameSettings.IsActive.Value)
        //{
        //    throw new ApiException(
        //        ApiExceptionCodeTypes.OperationNotAllowed,
        //        "The game is currently inactive and cannot be played. Please try again later."
        //    );
        //}

        var price = await _priceRepository.Query(p => p.Id == request.BetPriceId).FirstOrDefaultAsync();

        if (price == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Price with the specified ID: [{request.BetPriceId}] was not found.");
        }

        await _hubService.BetTransactionAsync(_gameInfoConfig.GameId, request.PromotionId, price.Value);

        var round = _configurationHolder.GetPrizeGroups(request.PromotionId).Cast<Round>().FirstOrDefault();
        var prize = GeneratorHolder.GetPrize<TPrize>(round!.Id);

        if (prize == null)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "The prize generation failed. No prize was generated for the specified criteria. Please try again or contact support."
            );
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(_gameInfoConfig.GameId, prize.CoinId, request.PromotionId, price.Multiplier * prize.Value);
        }

        //var @event = new UpdatePlayerExperienceEvent(Guid.NewGuid(), model.Amount, model.CurrencyId, _authService.GetCurrentPlayerId());
        //await _messageBus.Publish(@event);

        return new PlayResponseModel
        {
            IsWin = prize.Value > 0,
            PrizeId = prize.Id,
            WheelIndex = (prize as WheelPrize)?.WheelIndex,
            WinAmount = prize.Value > 0 ? price.Multiplier * prize.Value : null
        };
    }
}