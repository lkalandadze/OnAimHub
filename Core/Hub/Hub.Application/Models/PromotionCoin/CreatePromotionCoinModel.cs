using Hub.Application.Models.Withdraw.WithdrawOptionGroup;
using Hub.Domain.Enum;

namespace Hub.Application.Models.PromotionCoin;

public class CreatePromotionCoinModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsTemplate { get; set; }
    public IEnumerable<CreateWithdrawOptionGroupModel> WithdrawOptionGroups { get; set; }
}
