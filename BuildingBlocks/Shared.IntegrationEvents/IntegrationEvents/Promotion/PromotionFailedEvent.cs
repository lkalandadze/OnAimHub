using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Promotion;

public class PromotionFailedEvent : IPromotionFailedEvent
{
    public int PromotionId { get; set; }
    public string ErrorMessage { get; set; }
}
