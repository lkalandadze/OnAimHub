#nullable disable

namespace GameLib.Application.Models.Currency;

public class CurrencyBaseGetModel
{
    public string Id { get; set; }

    public static CurrencyBaseGetModel MapFrom(Domain.Entities.Currency currency)
    {
        return new CurrencyBaseGetModel
        {
            Id = currency.Id,
        };
    }
}