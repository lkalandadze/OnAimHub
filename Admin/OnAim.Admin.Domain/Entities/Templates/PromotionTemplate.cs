using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.HubEntities.Models;
using OnAim.Admin.Contracts.Dtos.Game;

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
    public IEnumerable<string>? SegmentIds { get;  set; }
    public ICollection<Coin>? Coins { get; set; } = new List<Coin>(); //coin with cointemplate id
    public ICollection<LeaderboardRecord>? Leaderboards { get; set; } = new List<LeaderboardRecord>(); //Leaderboard
    public ICollection<GameConfigurationTemplate>? Games { get; set; } = new List<GameConfigurationTemplate>();
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public int Usage { get; set; }

    public void UpdateCoins(IEnumerable<Coin> coins)
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

    public void AddCoins(IEnumerable<Coin> coins)
    {
        foreach (var coin in coins)
        {
            if (!Coins.Contains(coin))
            {
                Coins.Add(coin);
            }
        }
    }

    public void AddLeaderboard(IEnumerable<LeaderboardRecord> leaderboards)
    {
        foreach (var item in leaderboards)
        {
            if (!Leaderboards.Contains(item))
            {
                Leaderboards.Add(item);
            }
        }
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
public record CreatePromotionTemplate(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreateCoinModel>? Coins,
    IEnumerable<CreateLeaderboardRecordDto>? Leaderboards,
    IEnumerable<CreateGameConfigurationTemplateDto> Games
    );