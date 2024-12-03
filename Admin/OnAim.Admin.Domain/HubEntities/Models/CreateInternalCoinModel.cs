namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateInternalCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInternalCoinModel()
    {
        CoinType = Enum.CoinType.Internal;
    }
}
