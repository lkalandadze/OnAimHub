#nullable disable

namespace GameLib.Application.Models.Price;

public class PriceBaseGetModel
{
    public string BetPriceId { get; set; }
    public decimal Bet { get; set; }
    public string Coin { get; set; }

    public static PriceBaseGetModel MapFrom(Domain.Entities.Price entity)
    {
        return new PriceBaseGetModel
        {
            BetPriceId = entity.Id,
            Bet = entity.Value,
            Coin = entity.CoinId.Split('_')[1]
        };
    }
}