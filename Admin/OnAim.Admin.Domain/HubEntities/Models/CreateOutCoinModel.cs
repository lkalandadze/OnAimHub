namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateOutCoinModel : CreateCoinModel
{
    public IEnumerable<int> WithdrawOptionIds { get; set; }
    public IEnumerable<int> WithdrawOptionGroupIds { get; set; }

    public CreateOutCoinModel()
    {
        CoinType = Enum.CoinType.Out;
    }
}
