using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Aggregation;

public class AggregatedEvent : IIntegrationEvent
{
    public string PlayerId { get; set; }
    public DateTime Timestamp { get; set; }
    public double AddedPoints { get; set; }
    public string PromotionId { get; set; }
    public string ConfigKey { get; set; } // CoinIn
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
}
