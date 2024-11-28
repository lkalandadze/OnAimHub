#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.PromotionCoin;

public class CreatePromotionIncomingCoinModel : BaseCreatePromotionCoinModel
{
    // should be add configuration in future

    public CreatePromotionIncomingCoinModel()
    {
        CoinType = CoinType.Incomming;
    }
}