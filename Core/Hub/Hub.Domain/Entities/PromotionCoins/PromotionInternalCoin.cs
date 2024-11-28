using Hub.Domain.Enum;

namespace Hub.Domain.Entities.PromotionCoins;

public class PromotionInternalCoin : PromotionCoin
{
    public PromotionInternalCoin()
    {
        
    }

    public PromotionInternalCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, CoinType.Internal, promotionId)
    {
    }
}