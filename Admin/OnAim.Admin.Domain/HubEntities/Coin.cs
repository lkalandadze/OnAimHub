using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.HubEntities;

public class Coin
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int PromotionId { get; set; }
    public CoinType CoinType { get; set; }
}
//public class CoinType : DbEnum<string, CoinType>
//{
//    public static CoinType CoinIn => FromId(nameof(CoinIn));
//    public static CoinType CoinOut => FromId(nameof(CoinOut));
//    public static CoinType InternalCoin => FromId(nameof(InternalCoin));
//    public static CoinType Prize => FromId(nameof(Prize));
//}
public enum CoinType
{
    CoinIn,
    CoinOut
}