#nullable disable

using Hub.Application.Models.Withdraw.WithdrawOption;
using Hub.Domain.Enum;

namespace Hub.Application.Models.PromotionCoin;

public class CreatePromotionOutgoingCoinModel : BaseCreatePromotionCoinModel
{
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }

    public CreatePromotionOutgoingCoinModel()
    {
        CoinType = CoinType.Outgoing;
    }
}