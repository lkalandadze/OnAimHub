#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateInCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInCoinModel()
    {
        CoinType = CoinType.In;
    }
}