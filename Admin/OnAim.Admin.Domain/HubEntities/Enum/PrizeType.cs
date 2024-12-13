namespace OnAim.Admin.Domain.HubEntities.Enum;

public class PrizeType : DbEnum<string, PrizeType>
{
    public static PrizeType Car => FromId(nameof(Car));
    public static PrizeType House => FromId(nameof(House));

    public string CoinId { get; set; }
    public Coin.Coin Coin { get; set; }
}