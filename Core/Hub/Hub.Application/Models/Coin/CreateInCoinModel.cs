#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateInCoinModel : CreateCoinModel
{
    public string TemplateId { get; set; }
    // should be add configuration in future

    public CreateInCoinModel()
    {
        CoinType = CoinType.In;
    }
}