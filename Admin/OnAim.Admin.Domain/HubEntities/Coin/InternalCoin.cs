namespace OnAim.Admin.Domain.HubEntities.Coin;

public class InternalCoin : Coin
{
    public InternalCoin(){}

    public InternalCoin(string id, string name, string description, string imageUrl, int promotionId, int? templateId = null)
        : base(id, name, description, imageUrl, Enum.CoinType.Internal)
    {
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public int? FromTemplateId { get; private set; }
    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}
