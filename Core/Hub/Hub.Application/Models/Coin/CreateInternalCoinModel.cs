#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateInternalCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInternalCoinModel()
    {
        CoinType = CoinType.Internal;
    }
}