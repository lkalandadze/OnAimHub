using MongoDB.Bson;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CreateCoinTemplateDto
{
    public string Name { get; set; }
    public   string? Description { get; set; }
    public string ImageUrl { get; set; }
    public   CoinType CoinType { get; set; }
    public IEnumerable<int>? WithdrawOptionIds { get; set; }
}
public class WithdrawOptionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public int? WithdrawEndpointTemplateId { get; set; }
    public WithdrawEndpointTemplateDto WithdrawEndpointTemplate { get; set; }

    public ICollection<WithdrawOptionGroupDto> WithdrawOptionGroups { get; set; }
    public ICollection<PromotionCoinDto> PromotionCoins { get; set; }
    public ICollection<CoinTemplateDto> CoinTemplates { get; set; }
}
public class WithdrawEndpointTemplateDto
{
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
}
public class CoinTemplateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public ICollection<WithdrawOptionDto>? WithdrawOptions { get; set; }
}

public class UpdateCoinTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public IEnumerable<int>? WithdrawOptionIds { get; set; }
}