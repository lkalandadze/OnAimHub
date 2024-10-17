using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class LevelPrize : BaseEntity<int>
{
    public LevelPrize(int amount, string currencyId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        CurrencyId = currencyId;
        PrizeDeliveryType = prizeDeliveryType;
    }

    public int Amount { get; set; }
    public string CurrencyId { get; set; }
    public PrizeDeliveryType PrizeDeliveryType { get; set; }
    public int LevelId { get; set; }

    public void Update(int amount, string currencyId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        CurrencyId = currencyId;
        PrizeDeliveryType = prizeDeliveryType;
    }
}
