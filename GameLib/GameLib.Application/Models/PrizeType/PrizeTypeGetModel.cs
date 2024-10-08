#nullable disable

using GameLib.Application.Models.Currency;

namespace GameLib.Application.Models.PrizeType;

public class PrizeTypeGetModel : PrizeTypeBaseGetModel
{
    public CurrencyBaseGetModel Currency { get; set; }

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
            model.Currency = prizeType.Currency != null ? CurrencyBaseGetModel.MapFrom(prizeType.Currency) : default;
        }

        return model;
    }
}