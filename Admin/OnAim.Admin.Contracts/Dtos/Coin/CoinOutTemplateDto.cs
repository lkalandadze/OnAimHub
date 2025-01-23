using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinOutTemplateDto : CoinTemplateDto
{
    public List<WithdrawOptionCoinTempDto> WithdrawOptions { get; set; }
    public List<WithdrawOptionGroupCoinTempDto> WithdrawOptionGroups { get; set; }
}
