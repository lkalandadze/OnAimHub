#nullable disable

namespace GameLib.Application.Models.Price;

public class PriceBaseGetModel
{
    public string Coin { get; set; }

    public static PriceBaseGetModel MapFrom(Domain.Entities.Price entity)
    {
        return new PriceBaseGetModel
        {
            Coin = entity.CoinId.Split('_')[1]
        };
    }
}