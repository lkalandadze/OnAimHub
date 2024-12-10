#nullable disable

using Hub.Domain.Entities.Coins;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class PrizeType : DbEnum<string, PrizeType>
{
    public static PrizeType Car => FromId(nameof(Car));
    public static PrizeType House => FromId(nameof(House));

    public string CoinId { get; set; }
    public Coin Coin { get; set; }
}