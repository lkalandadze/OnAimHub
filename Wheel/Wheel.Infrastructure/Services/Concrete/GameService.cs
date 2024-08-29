using Shared.Application.Holders;
using Shared.Application.Services.Abstract;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Wheel.Application.Models;
using Wheel.Application.Models.Player;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.Services.Abstract;

namespace Wheel.Infrastructure.Services.Concrete;

public class GameService : IGameService
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IGameVersionRepository _gameVersionRepository;

    public GameService(
        GeneratorHolder generatorHolder,
        ConfigurationHolder configurationHolder,
        IAuthService authService,
        IHubService hubService,
        IConfigurationRepository configurationRepository,
        IGameVersionRepository gameVersionRepository)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _authService = authService;
        _hubService = hubService;
        _configurationRepository = configurationRepository;
        _gameVersionRepository = gameVersionRepository;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public GameVersionResponseModel GetGame()
    {
        var gameVersion = _gameVersionRepository.Query()
                                                .FirstOrDefault();

        if (gameVersion == null)
        {
            throw new InvalidOperationException("No active game version found for the provided SegmentIds.");
        }

        return new GameVersionResponseModel
        {
            Id = gameVersion.Id,
            Name = gameVersion.Name,
            IsActive = gameVersion.IsActive,
            SegmentIds = gameVersion.SegmentIds.ToList(),
            ActivationTime = DateTime.UtcNow,
        };
    }

    public async Task<PlayResultModel> PlayJackpotAsync(PlayRequestModel command)
    {
        return await PlayAsync<JackpotPrize>(command);
    }

    public async Task<PlayResultModel> PlayWheelAsync(PlayRequestModel command)
    {
        return await PlayAsync<WheelPrize>(command);
    }

    private async Task<PlayResultModel> PlayAsync<TPrize>(PlayRequestModel command)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(command.GameVersionId);

        var ttt = _authService.GetCurrentPlayerSegmentIds().ToList()[0];

        var prize = GeneratorHolder.GetPrize<TPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(command.GameVersionId);
        }

        return new PlayResultModel
        {
            PrizeResults = new List<BasePrize> { prize },
            Multiplier = 0,
        };
    }
}