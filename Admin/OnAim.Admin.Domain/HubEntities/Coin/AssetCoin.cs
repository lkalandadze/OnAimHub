namespace OnAim.Admin.Domain.HubEntities.Coin;

public class AssetCoin : Coin
{
    public AssetCoin()
    {

    }

    public AssetCoin(string id, string name, string description, string imageUrl, int promotionId, int? templateId = null)
        : base(id, name, description, imageUrl, Enum.CoinType.Asset)
    {
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public int? FromTemplateId { get; private set; }
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}