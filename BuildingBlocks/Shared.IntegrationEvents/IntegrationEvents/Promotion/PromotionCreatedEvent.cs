using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Promotion;

public class PromotionCreatedEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; set; }
}
