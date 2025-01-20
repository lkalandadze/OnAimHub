namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateAssetCoinModel : CreateCoinModel
{
    public decimal Value { get; set; }
    public string TemplateId { get; set; }
    // should be add configuration in future

    public CreateAssetCoinModel()
    {
        CoinType = Domain.HubEntities.Enum.CoinType.Asset;
    }
}
