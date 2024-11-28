#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.PromotionCoin;

public class CreatePromotionInternalCoinModel : BaseCreatePromotionCoinModel
{
    // should be add configuration in future

    public CreatePromotionInternalCoinModel()
    {
        CoinType = CoinType.Internal;
    }
}