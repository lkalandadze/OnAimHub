using Shared.IntegrationEvents.Interfaces;

namespace Leaderboard.Domain.Events;

public class CreatePlayerEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; private set; }
    public int PlayerId {  get; private set; }
    public string UserName { get; private set; }

    public CreatePlayerEvent(Guid correlationId, int playerId, string userName)
    {
        CorrelationId = correlationId;
        PlayerId = playerId;
        UserName = userName;
    }
}
