namespace Shared.IntegrationEvents.IntegrationEvents.Promotion;

public interface IPromotionCompletedEvent
{
    public int PromotionId { get; set; }
}
