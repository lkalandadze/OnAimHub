using GameLib.Application.Configurations;
using GameLib.Application;
using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Infrastructure.Bus;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Wheel.Application.Services.Concrete;

public class WheelService : IWheelService
{
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IMessageBus _messageBus;
    private readonly GameSettings _gameSettings;
    private readonly GameInfoConfiguration _gameInfoConfig;

    public WheelService(
        IGameConfigurationRepository configurationRepository,
        IAuthService authService,
        IHubService hubService,
        IMessageBus messageBus,
        GameSettings gameSettings,
        IOptions<GameInfoConfiguration> gameInfoConfig)
    {
        _authService = authService;
        _hubService = hubService;
        _messageBus = messageBus;
        _gameSettings = gameSettings;
        _gameInfoConfig = gameInfoConfig.Value;
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<WheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel model)
        where TPrize : BasePrize
    {
        if (!_gameSettings.IsActive.Value)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.OperationNotAllowed,
                "The game is currently inactive and cannot be played. Please try again later."
            );
        }

        await _hubService.BetTransactionAsync(_gameInfoConfig.GameId, model.PromotionId, model.Amount);

        var prize = GeneratorHolder.GetPrize<TPrize>();

        if (prize == null)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "The prize generation failed. No prize was generated for the specified criteria. Please try again or contact support."
            );
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(_gameInfoConfig.GameId, model.PromotionId, model.Amount * prize.Value);
        }

        //var @event = new UpdatePlayerExperienceEvent(Guid.NewGuid(), model.Amount, model.CurrencyId, _authService.GetCurrentPlayerId());

        //await _messageBus.Publish(@event);

        return new PlayResponseModel
        {
            //PrizeResults = [prize],
            Result = prize.Value <= 0 ? "You Lose" : $"Win Amount: {model.Amount * prize.Value}",
        };
    }
}