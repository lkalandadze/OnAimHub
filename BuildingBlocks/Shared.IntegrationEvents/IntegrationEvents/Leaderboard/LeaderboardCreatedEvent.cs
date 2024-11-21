using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

public class LeaderboardCreatedEvent : ILeaderboardCreatedEvent
{
    public Guid CorrelationId { get; set; }
}
