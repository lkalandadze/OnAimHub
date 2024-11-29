using Hub.Domain.Enum;

namespace Hub.Domain.Entities.Coins;

public class InCoin : Coin
{
    public InCoin()
    {
        
    }

    public InCoin(
        string id,
        string name,
        string description,
        string imageUrl,
        int promotionId) : base(id, name, description, imageUrl, CoinType.In, promotionId)
    {
    }
}