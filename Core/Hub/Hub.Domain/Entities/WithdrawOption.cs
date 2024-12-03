#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOption : BaseEntity<int>
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

    public void Update(
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

    public void AddWithdrawOptionGroups(IEnumerable<WithdrawOptionGroup> withdrawOptionGroups)
    {
        if (WithdrawOptionGroups == null)
        {
            WithdrawOptionGroups = [];
        }

        foreach (var withdrawOptionGroup in withdrawOptionGroups)
        {
            if (!WithdrawOptionGroups.Contains(withdrawOptionGroup))
            {
                WithdrawOptionGroups.Add(withdrawOptionGroup);
            }
        }
    }
}