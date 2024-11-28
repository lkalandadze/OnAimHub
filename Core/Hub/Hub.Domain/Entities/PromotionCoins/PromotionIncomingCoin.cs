using Hub.Domain.Enum;

namespace Hub.Domain.Entities.PromotionCoins;

public class PromotionIncomingCoin : PromotionCoin
{
    public PromotionIncomingCoin()
    {
        
    }

    public PromotionIncomingCoin(
        string id,
        string name,
        string description,
        string imageUrl,
        int promotionId) : base(id, name, description, imageUrl, CoinType.Incomming, promotionId)
    {
    }
}