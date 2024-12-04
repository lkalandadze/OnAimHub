namespace OnAim.Admin.Domain.HubEntities.Coin;

public class InternalCoin : Coin
{
    public InternalCoin(){}

    public InternalCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, Enum.CoinType.Internal, promotionId){}
}
