namespace OnAim.Admin.Domain.HubEntities.Coin;

public class AssetCoin : Coin
{
    public AssetCoin()
    {

    }

    public AssetCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, Enum.CoinType.Asset, promotionId)
    {
    }
}