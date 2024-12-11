using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

public class PlayLeaderboardEvent : IIntegrationEvent
{
    public PlayLeaderboardEvent(
        Guid correlationId,
        int leaderboardRecordId,
        int generatedAmount,
        int promotionId,
        int playerId
        )
    {
        CorrelationId = correlationId;
        LeaderboardRecordId = leaderboardRecordId;
        GeneratedAmount = generatedAmount;
        PromotionId = promotionId;
        PlayerId = playerId;
    }
    public Guid CorrelationId { get; set; }
    public int LeaderboardRecordId { get; set; }
    public int GeneratedAmount { get; set; }
    public int PromotionId { get; set; }
    public int PlayerId { get; set; }
}