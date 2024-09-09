using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;

    public GameService(
        GeneratorHolder generatorHolder,
        ConfigurationHolder configurationHolder,
        ISegmentRepository segmentRepository,
        IAuthService authService,
        IHubService hubService)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _segmentRepository = segmentRepository;
        _authService = authService;
        _hubService = hubService;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public GameResponseModel GetGame()
    {
        var segments = _segmentRepository.Query();

        return new GameResponseModel
        {
            Name = "Wheel",
            SegmentIds = segments == null ? default : segments.Select(x => x.Value).ToList(),
            ActivationTime = DateTime.UtcNow,
        };
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command)
    {
        return await PlayAsync<JackpotPrize>(command);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command)
    {
        return await PlayAsync<WheelPrize>(command);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel command)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(command.GameId);

        var ttt = _authService.GetCurrentPlayerSegmentIds().ToList()[0];

        var prize = GeneratorHolder.GetPrize<TPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(command.GameId);
        }

        return new PlayResponseModel
        {
            PrizeResults = new List<BasePrize> { prize },
            Multiplier = 0,
        };
    }
}