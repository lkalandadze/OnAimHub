#nullable disable

using Hub.Application.Models.Withdraw.WithdrawOption;
using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateOutCoinModel : CreateCoinModel
{
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }

    public CreateOutCoinModel()
    {
        CoinType = CoinType.Out;
    }
}