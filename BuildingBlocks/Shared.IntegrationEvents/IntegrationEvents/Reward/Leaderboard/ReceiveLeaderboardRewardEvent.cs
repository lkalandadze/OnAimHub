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
    public RewardDetail(int playerId, string coinId, decimal amount, int promotionId, int keyId, string sourceServiceName)
    {
        PlayerId = playerId;
        CoinId = coinId;
        Amount = amount;
        PromotionId = promotionId;
        KeyId = keyId;
        SourceServiceName = sourceServiceName;
    }

    public int PlayerId { get; set; }
    public int KeyId { get; set; }
    public string SourceServiceName { get; set; }
    public string CoinId { get; set; }
    public decimal Amount { get; set; }
    public int PromotionId { get; set; }
}