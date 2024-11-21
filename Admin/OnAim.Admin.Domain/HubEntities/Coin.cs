using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.HubEntities;

public class Coin : BaseEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int PromotionId { get; set; }
    public CoinType CoinType { get; set; }
}
public enum CoinType
{
    CoinIn,
    CoinOut
}