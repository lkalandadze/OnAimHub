using AggregationService.Domain.Entities;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinInTemplateDto : CoinTemplateDto
{
    public IEnumerable<AggregationConfiguration> Configurations { get; set; }
}