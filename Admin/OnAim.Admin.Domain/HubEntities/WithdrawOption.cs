namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOption : BaseEntity<int>
{
    public WithdrawOption()
    {
    }

    public WithdrawOption(
        string title,
        string description,
        string imageUrl,
        EndpointContentType contentType,
        string endpoint = null,
        string endpointContent = null,
        int? withdrawEndpointTemplateId = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        ContentType = contentType;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = withdrawEndpointTemplateId;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }
    public ICollection<PromotionCoin> PromotionCoins { get; set; }

    public int? WithdrawEndpointTemplateId { get; private set; }
    public WithdrawEndpointTemplate WithdrawEndpointTemplate { get; set; }
}
