#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Currency : DbEnum<string, Currency>
{
    public static Currency OnAimCoin => FromId(nameof(OnAimCoin));
    public static Currency FreeSpin => FromId(nameof(FreeSpin));

    public ICollection<Price> Prices { get; private set; } = [];
    public ICollection<PrizeType> PrizeTypes { get; private set; } = [];

    public void AddPrices(IEnumerable<Price> prices)
    {
        foreach (var price in prices)
        {
            Prices.Add(price);
        }
    }

    public void AddPrizeTypes(IEnumerable<PrizeType> prizeTypes)
    {
        foreach (var prizeType in prizeTypes)
        {
            PrizeTypes.Add(prizeType);
        }
    }
}