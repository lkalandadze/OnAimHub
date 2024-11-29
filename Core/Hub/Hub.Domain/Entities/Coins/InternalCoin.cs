using Hub.Domain.Enum;

namespace Hub.Domain.Entities.Coins;

public class InternalCoin : Coin
{
    public InternalCoin()
    {
        
    }

    public InternalCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, CoinType.Internal, promotionId)
    {
    }
}