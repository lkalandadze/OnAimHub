using Hub.Domain.Enum;

namespace Hub.Domain.Entities.PromotionCoins;

public class PromotionPrizeCoin : PromotionCoin
{
    public PromotionPrizeCoin()
    {
        
    }

    public PromotionPrizeCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, CoinType.Prize, promotionId)
    {
    }
}