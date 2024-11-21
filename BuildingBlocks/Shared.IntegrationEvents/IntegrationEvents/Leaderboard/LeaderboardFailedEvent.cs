namespace Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

public class LeaderboardFailedEvent : ILeaderboardFailedEvent
{
    public int PromotionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
}
