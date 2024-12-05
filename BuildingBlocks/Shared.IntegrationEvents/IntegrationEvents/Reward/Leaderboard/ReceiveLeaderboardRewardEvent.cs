using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;

public class ReceiveLeaderboardRewardEvent : IIntegrationEvent
{
    public ReceiveLeaderboardRewardEvent(Guid correlationId, List<RewardDetail> rewards, DateTime createdAt)
    {
        CorrelationId = correlationId;
        Rewards = rewards ?? new List<RewardDetail>();
        CreatedAt = createdAt;
    }

    public Guid CorrelationId { get; set; }
    public List<RewardDetail> Rewards { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RewardDetail
{
    public RewardDetail(int playerId, string coinId, decimal amount, int promotionId)
    {
        PlayerId = playerId;
        CoinId = coinId;
        Amount = amount;
        PromotionId = promotionId;
    }

    public int PlayerId { get; set; }
    public string CoinId { get; set; }
    public decimal Amount { get; set; }
    public int PromotionId { get; set; }
}