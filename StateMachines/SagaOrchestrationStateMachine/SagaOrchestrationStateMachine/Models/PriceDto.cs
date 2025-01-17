namespace SagaOrchestrationStateMachine.Models;

public class PriceDto
{
    public string Id { get; set; }
    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }
    public string CoinId { get; set; }
}
