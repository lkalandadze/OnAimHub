using AggregationService.Domain.Entities;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CreateCoinTemplateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public IEnumerable<int>? WithdrawOptionIds { get; set; }
    public IEnumerable<int>? WithdrawOptionGroupIds { get; set; }
    public IEnumerable<AggregationConfiguration>? Configurations { get; set; }
}
