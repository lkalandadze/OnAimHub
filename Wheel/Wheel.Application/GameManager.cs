using Shared.Application.Holders;
using Shared.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Wheel.Application;

public class GameManager
{
    private readonly Player _player;
    private readonly GeneratorHolder _holder;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IPrizeGroupRepository<WheelPrizeGroup> _prizeGroupRepository;

    public GameManager(Player player, GeneratorHolder holder, IConfigurationRepository configurationRepository, IPrizeGroupRepository<WheelPrizeGroup> prizeGroupRepository)
    {
        _player = player;
        _holder = holder;
        _configurationRepository = configurationRepository;
        _prizeGroupRepository = prizeGroupRepository;
    }

    public GameManager Start()
    {
        if (_player == null)
        {
            throw new InvalidOperationException();
        }

        return this;
    }

    public SpinResult Spin(decimal betAmount, int gameVersionId)
    {
        // ask hub if player has enough balance

        var configuration = _configurationRepository.Query(x => x.GameVersionId == gameVersionId && x.IsActive)
                                                    .FirstOrDefault();

        if (configuration == null)
        {
            throw new ArgumentNullException();
        }

        var prizeGroup = _prizeGroupRepository.Query(x => x.ConfigurationId == configuration.Id)
                                              .Include(x => x.Segment)
                                              .FirstOrDefault();

        if (prizeGroup == null)
        {
            throw new ArgumentNullException();
        }

        var prize = GeneratorHolder.GetPrize<WheelPrize>(configuration.Id, prizeGroup.SegmentId);

        var result = new SpinResult()
        {
            WinAmount = prize.Value,
        };

        return result;
    }
}

public class Player
{
    public int Id { get; set; }
    public string NickName { get; set; }
    public Balance Balance { get; set; }
}

public class Balance
{
    public decimal WheelBalance { get; set; }
}

public class SpinResult
{
    public int WinAmount { get; set; }
}