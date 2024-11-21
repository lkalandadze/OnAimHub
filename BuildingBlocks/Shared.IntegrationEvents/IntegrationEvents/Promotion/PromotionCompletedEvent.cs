namespace Shared.IntegrationEvents.IntegrationEvents.Promotion;

public class PromotionCompletedEvent : IPromotionCompletedEvent
{
    public int PromotionId { get; set; }
}
