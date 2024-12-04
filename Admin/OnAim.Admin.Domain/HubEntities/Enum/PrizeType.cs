namespace OnAim.Admin.Domain.HubEntities.Enum;

public class PrizeType : DbEnum<string, PrizeType>
{
    public static PrizeType Car => FromId(nameof(Car));
    public static PrizeType House => FromId(nameof(House));
    public static PrizeType Coin => FromId(nameof(Coin));

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }
}