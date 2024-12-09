#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Coin : DbEnum<string, Coin>
{
    public static Coin OnAimCoin => FromId(nameof(OnAimCoin));
    public static Coin FreeSpin => FromId(nameof(FreeSpin));

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