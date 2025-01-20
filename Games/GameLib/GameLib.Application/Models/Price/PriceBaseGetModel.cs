#nullable disable

namespace GameLib.Application.Models.Price;

public class PriceBaseGetModel
{
    public int BetPriceId { get; set; }
    public decimal BetAmount { get; set; }
    public string Coin { get; set; }

    public static PriceBaseGetModel MapFrom(Domain.Entities.Price entity)
    {
        return new PriceBaseGetModel
        {
            BetPriceId = entity.Id,
            BetAmount = entity.Value,
            Coin = entity.CoinId.Split('_')[1]
        };
    }
}