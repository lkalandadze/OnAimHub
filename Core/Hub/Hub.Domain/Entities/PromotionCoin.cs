using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PromotionCoin : BaseEntity<string>
{
    public PromotionCoin()
    {
        
    }
    public PromotionCoin(string id, string name, string imageUrl, int promotionId, Promotion promotion, bool isDeleted, DateTimeOffset? dateDeleted)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        PromotionId = promotionId;
        Promotion = promotion;
        IsDeleted = isDeleted;
        DateDeleted = dateDeleted;
    }

    public CoinType CoinType { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
