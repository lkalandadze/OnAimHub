#nullable disable

using PenaltyKicks.Domain.Enums;
using Shared.Lib.Attributes;
using Shared.Domain.Entities;

namespace PenaltyKicks.Domain.Entities;

public class PenaltyGame : BaseEntity<int>
{
    public PenaltyGame()
    {
        
    }

    public PenaltyGame(
        int playerId, 
        int betPriceId, 
        int prizeId,
        int prizeValue,
        decimal priceMultiplier,
        string coinId,
        List<bool> kickSequence)
    {
        PlayerId = playerId;
        BetPriceId = betPriceId;
        PrizeId = prizeId;
        PrizeValue = prizeValue;
        PriceMultiplier = priceMultiplier;
        CoinId = coinId;
        KickSequence = kickSequence;
        GoalsScored = 0;
        CurrentKickIndex = 0;
        GameState = GameState.InProgress;
        StartDate = DateTime.UtcNow;
    }

    public int PlayerId { get; private set; }
    public int BetPriceId { get; private set; }
    public int PrizeId { get; private set; }
    public int PrizeValue { get; private set; }
    public decimal PriceMultiplier { get; private set; }
    public string CoinId { get; private set; }

    //[ListToStringConverter<bool>]
    public List<bool> KickSequence { get; private set; }

    public int GoalsScored { get; private set; }
    public int CurrentKickIndex { get; private set; }
    public GameState GameState { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public bool IsFinished { get; private set; }

    public void FinishGame()
    {
        IsFinished = true;
        EndDate = DateTime.UtcNow;
    }

    public void IncreaseGoalsScored(int increment)
    {
        GoalsScored += increment;
    }

    public void IncreaseCurrentKickIndex(int increment)
    {
        CurrentKickIndex += increment;
    }
}