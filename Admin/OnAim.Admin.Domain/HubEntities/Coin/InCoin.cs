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
        int promotionId,
        int? templateId = null) : base(id, name, description, imageUrl, Enum.CoinType.In)
    {
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public int? FromTemplateId { get; private set; }
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}
