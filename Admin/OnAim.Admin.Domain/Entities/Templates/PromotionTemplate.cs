using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnAim.Admin.Domain.Entities.Templates;

public class PromotionTemplate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get;  set; }
    public DateTime EndDate { get;  set; }
    public IEnumerable<string> SegmentIds { get;  set; }
    public ICollection<CoinTemplate>? Coins { get; set; } = new List<CoinTemplate>();
    public ICollection<LeaderboardTemplate>? Leaderboards { get; set; } = new List<LeaderboardTemplate>();
    public ICollection<GameConfigurationTemplate> Games { get; set; } = new List<GameConfigurationTemplate>();

    public void UpdateCoins(IEnumerable<CoinTemplate> coins)
    {
        if (coins != null)
        {
            Coins.Clear();

            foreach (var option in coins)
            {
                Coins.Add(option);
            }
        }
    }

    public void AddCoins(IEnumerable<CoinTemplate> coins)
    {
        foreach (var coin in coins)
        {
            if (!Coins.Contains(coin))
            {
                Coins.Add(coin);
            }
        }
    }

    public void AddLeaderboard(IEnumerable<LeaderboardTemplate> leaderboards)
    {
        foreach (var item in leaderboards)
        {
            if (!Leaderboards.Contains(item))
            {
                Leaderboards.Add(item);
            }
        }
    }
}