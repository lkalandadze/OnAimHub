using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class PlayerReward : BaseEntity<int>
{
    public PlayerReward(int playerId, int levelPrizeId)
    {
        PlayerId = playerId;
        LevelPrizeId = levelPrizeId;
        RewardStatus = RewardStatus.InProgress;
    }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public int LevelPrizeId { get; set; }
    public LevelPrize LevelPrize { get; set; }

    public RewardStatus RewardStatus { get; set; }

    public DateTimeOffset? DateClaimed { get; private set; }
}