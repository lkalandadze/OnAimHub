using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOption
{
    public WithdrawOption()
    {
    }

    public WithdrawOption(
        string title,
        string description,
        string imageUrl,
        string endpoint = null,
        EndpointContentType? contentType = null,
        string endpointContent = null,
        int? withdrawOptionEndpointId = null,
        IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        ContentType = contentType.Value;
        EndpointContent = endpointContent;
        WithdrawOptionEndpointId = withdrawOptionEndpointId;
        WithdrawOptionGroups = withdrawOptionGroups?.ToList();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public int? WithdrawOptionEndpointId { get; private set; }
    public WithdrawOptionEndpoint WithdrawOptionEndpoint { get; private set; }

    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; private set; }
    public ICollection<OutCoin> OutCoins { get; private set; }
}
