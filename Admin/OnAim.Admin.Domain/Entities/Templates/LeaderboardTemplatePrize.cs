using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.Entities.Templates;

public class LeaderboardTemplatePrize
{
    public LeaderboardTemplatePrize(){}

    public LeaderboardTemplatePrize(int startRank, int endRank, string coinId, int amount)
    {
        Id = ObjectId.GenerateNewId().ToString();
        StartRank = startRank;
        EndRank = endRank;
        CoinId = coinId; //coin with cointemplate id
        Amount = amount;
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
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }

    public void Update(int startRank, int endRank, string coinId, int amount)
    {
        StartRank = startRank;
        EndRank = endRank;
        CoinId = coinId;
        Amount = amount;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
