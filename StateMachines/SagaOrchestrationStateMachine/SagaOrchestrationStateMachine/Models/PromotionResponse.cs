namespace SagaOrchestrationStateMachine.Models;

public class PromotionResponse
{
    public int PromotionId { get; set; }
    public List<PromotionResponseCoin> Coins { get; set; }
}
