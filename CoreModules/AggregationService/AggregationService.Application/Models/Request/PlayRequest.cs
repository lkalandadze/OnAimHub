namespace AggregationService.Application.Models.Request;

public class PlayRequest
{
    public int PlayerId { get; set; }
    public string CoinIn { get; set; }
    public decimal Amount { get; set; }
    public int PromotionId { get; set; }
}
