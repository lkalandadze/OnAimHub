using Shared.Application.Holders;
using Shared.Application.Services.Abstract;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Wheel.Application.Models;
using Wheel.Application.Models.Player;
using Wheel.Domain.Entities;

namespace Wheel.Application;

public class GameManager
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IHubService _hubService;
    private readonly IAuthService _authService;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IPrizeGroupRepository<WheelPrizeGroup> _prizeGroupRepository;

    public GameManager(
        GeneratorHolder generatorHolder, 
        ConfigurationHolder configurationHolder,
        IHubService hubService,
        IAuthService authService, 
        IConfigurationRepository configurationRepository, 
        IPrizeGroupRepository<WheelPrizeGroup> prizeGroupRepository)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _hubService = hubService;
        _authService = authService;
        _configurationRepository = configurationRepository;
        _prizeGroupRepository = prizeGroupRepository;
    }

    public InitialDataResponseModel GetInitialData()
    {

        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public async Task<PlayResultModel> WheelPlayAsync(PlayRequestModel command)
    {
        await _hubService.BetTransactionAsync(command.GameVersionId);

        var prize = GeneratorHolder.GetPrize<WheelPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentId());

        if (prize == null)
        {
            throw new ArgumentNullException();
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(command.GameVersionId);
        }

        return new PlayResultModel
        {
            PrizeResults = new List<BasePrize> { prize },
            BetTransactionId = 0,
            Multiplier = 0,
        };
    }
}