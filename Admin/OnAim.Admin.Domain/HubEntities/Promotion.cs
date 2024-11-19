using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;

namespace OnAim.Admin.Domain.HubEntities;

public class Promotion
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public PromotionStatus PromotionStatus { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public List<Coin> Coins { get; set; } = new List<Coin>();

    public void AddCoin(Coin coin)
    {
        if (coin.CoinType == CoinType.CoinIn && Coins.Any(c => c.CoinType == CoinType.CoinIn))
            throw new BadRequestException("Only one CoinIn is allowed per promotion.");
        if (coin.CoinType == CoinType.CoinOut && Coins.Any(c => c.CoinType == CoinType.CoinOut))
            throw new BadRequestException("Only one CoinOut is allowed per promotion.");

        Coins.Add(coin);
    }
}
//public class PromotionStatus : DbEnum<string, PromotionStatus>
//{
//    public static PromotionStatus Active => FromId(nameof(Active));
//    public static PromotionStatus Finished => FromId(nameof(Finished));
//    public static PromotionStatus Cancelled => FromId(nameof(Cancelled));
//}