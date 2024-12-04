namespace OnAim.Admin.Domain.HubEntities.Coin;
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
        int promotionId) : base(id, name, description, imageUrl, Enum.CoinType.In, promotionId)
    {
    }
}
