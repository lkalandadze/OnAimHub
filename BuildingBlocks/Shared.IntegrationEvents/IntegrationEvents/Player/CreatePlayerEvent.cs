using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Player;

public class CreatePlayerEvent : IIntegrationEvent
{
    public CreatePlayerEvent(
        Guid correlationId,
        int playerId,
        string userName
        )
    {
        CorrelationId = correlationId;
        PlayerId = playerId;
        UserName = userName;
    }
    public Guid CorrelationId { get; set; }
    public int PlayerId { get; set; }
    public string UserName { get; set; }
}