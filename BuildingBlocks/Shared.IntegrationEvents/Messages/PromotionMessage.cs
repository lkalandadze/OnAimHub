using Shared.IntegrationEvents.Messages.Interfaces;

namespace Shared.IntegrationEvents.Messages;

public class PromotionMessage : IPromotionMessage
{
    public PromotionMessage()
    {
        
    }
    public int PromotionId { get; set; }
}
