namespace OnAim.Admin.Domain.HubEntities.Coin;

public class AssetCoin : Coin
{
    public AssetCoin()
    {

    }

    public AssetCoin(string id, string name, string description, string imageUrl, int promotionId, int? templateId = null)
        : base(id, name, description, imageUrl, Domain.HubEntities.Enum.CoinType.Asset, promotionId, templateId)
    {
    }
}