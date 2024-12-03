namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateOutCoinModel : CreateCoinModel
{
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }

    public CreateOutCoinModel()
    {
        CoinType = Enum.CoinType.Out;
    }
}
