using OnAim.Admin.Domain.HubEntities;

public class AssetCoin : Coin
{
    public AssetCoin()
    {

    }

    public AssetCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, CoinType.Asset, promotionId)
    {
    }
}
