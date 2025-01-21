namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionResponse
{
    public int PromotionId { get; set; }
    public List<PromotionResponseCoin> Coins { get; set; }
}
