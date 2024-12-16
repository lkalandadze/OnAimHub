using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinTemplateListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CoinType CoinType { get; set; }
    public string ImgUrl { get; set; }
    public bool IsDeleted { get; set; }
    public List<WithdrawOptionCoinTempDto> WithdrawOptions { get; set; }
    public List<WithdrawOptionGroupCoinTempDto> WithdrawOptionGroups { get; set; }
}