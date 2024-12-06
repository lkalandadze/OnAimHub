using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Player;
using TestWheel.Application.Models.Player;
using TestWheel.Application.Services.Abstract;
using TestWheel.Domain.Entities;

namespace TestWheel.Application.Services.Concrete;

public class TestWheelService : ITestWheelService
{
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IMessageBus _messageBus;

    public TestWheelService(
        IGameConfigurationRepository configurationRepository,
        IAuthService authService,
        IHubService hubService,
        IMessageBus messageBus)
    {
        _authService = authService;
        _hubService = hubService;
        _messageBus = messageBus;
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel model)
    {
        return await PlayAsync<JackpotPrize>(model);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<TestWheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel model)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(model.GameId, model.CurrencyId, model.Amount);

        //TODO: prioritetis minicheba segmentistvis
        //var prize = GeneratorHolder.GetPrize<TPrize>(_authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        //if (prize == null)
        //{
        //    throw new ArgumentNullException(nameof(prize));
        //}

        //if (prize.Value > 0)
        //{
        //    await _hubService.WinTransactionAsync(model.GameId, model.CurrencyId, model.Amount);
        //}

        var @event = new UpdatePlayerExperienceEvent(Guid.NewGuid(), model.Amount, model.CurrencyId, _authService.GetCurrentPlayerId());

        await _messageBus.Publish(@event);

        return new PlayResponseModel
        {
            PrizeResults = null,
            Multiplier = 0,
        };
    }
}