#nullable disable

using GameLib.Application.Models.Coin;

namespace GameLib.Application.Models.PrizeType;

public class PrizeTypeGetModel : PrizeTypeBaseGetModel
{
    public CoinBaseGetModel Coin { get; set; }

    public static PrizeTypeGetModel MapFrom(Domain.Entities.PrizeType prizeType, bool includeNavProperties = true)
    {
        var model = new PrizeTypeGetModel
        {
            Id = prizeType.Id,
            Name = prizeType.Name,
            IsMultiplied = prizeType.IsMultiplied,
        };

        if (includeNavProperties)
        {
            model.Coin = prizeType.Coin != null ? CoinBaseGetModel.MapFrom(prizeType.Coin) : default;
        }

        return model;
    }
}