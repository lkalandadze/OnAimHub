using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class CreatePromotionCoinModel
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsTemplate { get; set; }
    public IEnumerable<CreateWithdrawOptionGroupModel> WithdrawOptionGroups { get; set; }
}
public class UpdatePromotionStatusDto 
{
    public int Id { get; set; }
    public PromotionStatus Status { get; set; }
}