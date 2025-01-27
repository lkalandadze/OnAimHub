using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionCoinDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    //public DateTimeOffset? DateDeleted { get; set; }

    //public int PromotionId { get; set; }

    //public ICollection<WithdrawOptionDto> WithdrawOptions { get; set; }
}
