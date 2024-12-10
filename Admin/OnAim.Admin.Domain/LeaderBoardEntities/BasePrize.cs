using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class BasePrize
{
    public BasePrize()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string CoinId { get; set; }
    public int Amount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
