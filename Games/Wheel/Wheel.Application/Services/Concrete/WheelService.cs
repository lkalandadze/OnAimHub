using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class WheelService : IWheelService
{
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;

    public WheelService(IAuthService authService, IHubService hubService)
    {
        _authService = authService;
        _hubService = hubService;
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel model)
    {
        return await PlayAsync<JackpotPrize>(model);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<WheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel model)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(model.GameId, model.CurrencyId, model.Amount);

        //TODO: prioritetis minicheba segmentistvis
        var prize = GeneratorHolder.GetPrize<TPrize>(_authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(model.GameId, model.CurrencyId, model.Amount);
        }

        return new PlayResponseModel
        {
            PrizeResults = [prize],
            Multiplier = 0,
        };
    }
}