namespace Shared.IntegrationEvents.IntegrationEvents.Promotion;

public interface IPromotionFailedEvent
{
    public int PromotionId { get; set; }
    public string ErrorMessage { get; set; }
}
