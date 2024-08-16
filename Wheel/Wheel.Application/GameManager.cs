using Shared.Application.Holders;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
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

    public InitialDataResponse GetInitialData()
    {
        return new InitialDataResponse
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public PlayResult Play(PlayCommand command)
    {
        // make bet transaction

        var prize = GeneratorHolder.GetPrize<WheelPrize>(command.GameVersionId, command.SegmentId);

        // make win transaction

        return new PlayResult
        {
            PrizeResults = new List<BasePrize> { prize },
            BetTransactionId = 0,
            Multiplier = 0,
        };
    }
}

public class Player
{
    public int Id { get; set; }
    public string NickName { get; set; }
    public Dictionary<string, double> Balances { get; set; }
}

public class PlayResult
{
    //public abstract SubGameTypes SubGameType { get; }
    public List<BasePrize> PrizeResults { get; set; }
    //public List<MissionsResultDto> MissionsResults { get; set; } = new();
    //public List<Suits> CompletedChanceSymbols { get; set; } = new();
    //public List<ChanceJackpotPrizeDto> WonChanceJackpotPrizes { get; set; } = new();
    internal long BetTransactionId { get; set; }
    public int Multiplier { get; set; }
}

public class PlayCommand
{
    public int GameVersionId { get; set; }
    public int SegmentId { get; set; }
}

public class InitialDataResponse
{
    public Dictionary<string, List<BasePrizeGroup>> PrizeGroups { get; set; }
    public IEnumerable<Price> Prices { get; set; }
}