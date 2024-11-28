#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.PromotionCoin;

public class CreatePromotionPrizeCoinModel : BaseCreatePromotionCoinModel
{
    // should be add configuration in future

    public CreatePromotionPrizeCoinModel()
    {
        CoinType = CoinType.Prize;
    }
}