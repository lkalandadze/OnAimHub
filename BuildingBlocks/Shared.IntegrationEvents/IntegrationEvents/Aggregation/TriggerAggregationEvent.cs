using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Aggregation;

public class TriggerAggregationEvent : IIntegrationEvent
{
    public TriggerAggregationEvent(
        Guid correlationId,
        int playerId,
        string coinIn,
        decimal amount,
        int promotionId
        )
    {
        CorrelationId = correlationId;
        PlayerId = playerId;
        CoinIn = coinIn;
        Amount = amount;
        PromotionId = promotionId;
    }
    public Guid CorrelationId { get; set; }
    public int PlayerId { get; set; }
    public string CoinIn { get; set; }
    public decimal Amount { get; set; }
    public int PromotionId { get; set; }
}