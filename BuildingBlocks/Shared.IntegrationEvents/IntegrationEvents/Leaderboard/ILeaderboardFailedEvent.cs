using MassTransit;

namespace Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

public interface ILeaderboardFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; set; }
}
