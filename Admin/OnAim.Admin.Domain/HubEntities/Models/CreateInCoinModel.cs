namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateInCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInCoinModel()
    {
        CoinType = Enum.CoinType.In;
    }
}
