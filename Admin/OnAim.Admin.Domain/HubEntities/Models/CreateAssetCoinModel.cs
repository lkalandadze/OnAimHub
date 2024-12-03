namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateAssetCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateAssetCoinModel()
    {
        CoinType = Enum.CoinType.Asset;
    }
}
