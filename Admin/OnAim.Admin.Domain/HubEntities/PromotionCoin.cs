using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.HubEntities;

public class PromotionCoin
{
    public PromotionCoin() {}

    public PromotionCoin(
        string id,
        string name,
        string description,
        string imageUrl,
        CoinType coinType,
        int promotionId)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }
}
public enum CoinType
{
    Default = 0,
    Incomming = 1,
    Outgoing = 2,
    Internal = 3,
    Prize = 4,
}
public class PromotionIncomingCoin : PromotionCoin
{
}
public class PromotionInternalCoin : PromotionCoin
{
}
public class PromotionOutgoingCoin : PromotionCoin
{
    public ICollection<WithdrawOption> WithdrawOptions { get; private set; }
}
public class PromotionPrizeCoin : PromotionCoin
{
}