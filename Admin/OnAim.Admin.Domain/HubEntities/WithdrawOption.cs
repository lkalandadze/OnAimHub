using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOption
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public int? WithdrawEndpointTemplateId { get; private set; }
    public WithdrawOptionEndpoint WithdrawEndpointTemplate { get; set; }

    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }
    public ICollection<PromotionOutgoingCoin> PromotionCoins { get; set; }
    public ICollection<CoinTemplate> CoinTemplates { get; set; }
}
