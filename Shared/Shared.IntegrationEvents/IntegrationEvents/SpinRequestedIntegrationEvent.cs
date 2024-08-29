using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents;

public class SpinRequestedIntegrationEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; } = 10.00m; // Each spin costs 10 GEL

    public SpinRequestedIntegrationEvent(Guid correlationId, Guid userId)
    {
        CorrelationId = correlationId;
        UserId = userId;
    }
}