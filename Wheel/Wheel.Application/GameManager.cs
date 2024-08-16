using Shared.Application.Holders;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Wheel.Application.Models;
using Wheel.Domain.Entities;

namespace Wheel.Application;

public class GameManager
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IPrizeGroupRepository<WheelPrizeGroup> _prizeGroupRepository;

    public GameManager(GeneratorHolder generatorHolder, ConfigurationHolder configurationHolder, IConfigurationRepository configurationRepository, IPrizeGroupRepository<WheelPrizeGroup> prizeGroupRepository)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
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

    public PlayResultModel Play(PlayRequestModel command)
    {
        // make bet transaction

        var prize = GeneratorHolder.GetPrize<JackpotPrize>(command.GameVersionId, command.SegmentId);

        // make win transaction

        return new PlayResultModel
        {
            PrizeResults = new List<BasePrize> { prize },
            BetTransactionId = 0,
            Multiplier = 0,
        };
    }
}