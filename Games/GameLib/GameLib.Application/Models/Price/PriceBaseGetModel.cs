#nullable disable

namespace GameLib.Application.Models.Price;

public class PriceBaseGetModel
{
    public string CoinId { get; set; }

    public static PriceBaseGetModel MapFrom(Domain.Entities.Price entity)
    {
        return new PriceBaseGetModel
        {
            CoinId = entity.CoinId
        };
    }
}