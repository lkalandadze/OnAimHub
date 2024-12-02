using OnAim.Admin.Domain.HubEntities;

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
