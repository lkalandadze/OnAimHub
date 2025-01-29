using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public decimal? Value { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
