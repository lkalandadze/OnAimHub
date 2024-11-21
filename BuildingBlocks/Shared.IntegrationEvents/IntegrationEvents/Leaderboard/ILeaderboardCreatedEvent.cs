using MassTransit;

namespace Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

public interface ILeaderboardCreatedEvent : CorrelatedBy<Guid>
{
}
