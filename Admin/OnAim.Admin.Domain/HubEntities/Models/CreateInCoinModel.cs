using AggregationService.Domain.Entities;

namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateInCoinModel : CreateCoinModel
{
    public IEnumerable<AggregationConfiguration> Configurations { get; set; }

    public CreateInCoinModel()
    {
        CoinType = Enum.CoinType.In;
    }
}
