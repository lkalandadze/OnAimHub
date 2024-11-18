using System.Numerics;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PromotionStatistics TotalPromotion { get; set; }
    public PromotionStatistics CompletePromotion { get; set; }
    public PromotionStatistics InProgressPromotion { get; set; }
}
public class PromotionStatistics
{
    public int Promotions { get; set; }
    public decimal Value { get; set; }
}