using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class LevelPrize : BaseEntity<int>
{
    public LevelPrize(int amount, string currencyId, PrizeType prizeType)
    {
        Amount = amount;
        CurrencyId = currencyId;
        PrizeType = prizeType;
    }

    public int Amount { get; set; }
    public string CurrencyId { get; set; }
    public PrizeType PrizeType { get; set; }
    public int LevelId { get; set; }

    public void Update(int amount, string currencyId, PrizeType prizeType)
    {
        Amount = amount;
        CurrencyId = currencyId;
        PrizeType = prizeType;
    }
}
