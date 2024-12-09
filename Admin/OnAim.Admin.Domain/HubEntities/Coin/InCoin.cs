namespace OnAim.Admin.Domain.HubEntities.Coin;
public class InCoin : Coin
{
    public InCoin()
    {

    }

    public InCoin(string id, string name, string description, string imageUrl, int promotionId, int? templateId = null)
        : base(id, name, description, imageUrl, Domain.HubEntities.Enum.CoinType.In, promotionId, templateId)
    {
    }
}
