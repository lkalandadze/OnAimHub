using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PromotionCoinWithdrawOption : BaseEntity<int>
{
    public PromotionCoinWithdrawOption(string promotionCoinId, int withdrawOptionId)
    {
        PromotionCoinId = promotionCoinId;
        WithdrawOptionId = withdrawOptionId;
    }
    public string PromotionCoinId { get; set; }
    public PromotionCoin PromotionCoin { get; set; }

    public int WithdrawOptionId { get; set; }
    public WithdrawOption WithdrawOption { get; set; }
}