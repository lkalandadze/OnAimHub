namespace OnAim.Admin.Domain.HubEntities.Coin;

public class InternalCoin : Coin
{
    public InternalCoin()
    {

    }

    public InternalCoin(string id, string name, string description, string imageUrl, int promotionId, string? templateId = null)
        : base(id, name, description, imageUrl, Domain.HubEntities.Enum.CoinType.Internal, promotionId, templateId)
    {
    }
}
