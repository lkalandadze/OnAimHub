using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities.Coin;

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
    public ICollection<Coin> Coins { get;  set; }

    public void SetCoins(IEnumerable<Coin> coins)
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
}