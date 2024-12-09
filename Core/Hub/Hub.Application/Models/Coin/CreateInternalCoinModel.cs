#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateInternalCoinModel : CreateCoinModel
{
    public string TemplateId { get; set; }
    // should be add configuration in future

    public CreateInternalCoinModel()
    {
        CoinType = CoinType.Internal;
    }
}