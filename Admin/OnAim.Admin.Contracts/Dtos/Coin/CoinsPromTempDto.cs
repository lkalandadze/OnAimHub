using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinsPromTempDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImgUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DateDeleted { get; set; }
    public List<WithdrawOptionDto>? WithdrawOptions { get; set; }
    public List<WithdrawOptionGroupDto>? WithdrawOptiongroups { get; set; }
}
