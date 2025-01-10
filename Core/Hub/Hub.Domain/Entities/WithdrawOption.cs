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
        decimal value,
        string endpoint = null,
        EndpointContentType? contentType = null,
        string endpointContent = null, 
        int? withdrawOptionEndpointId = null,
        IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null,
        int? createdByUserId = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Value = value;
        Endpoint = endpoint;
        ContentType = contentType.Value;
        EndpointContent = endpointContent;
        WithdrawOptionEndpointId = withdrawOptionEndpointId;
        WithdrawOptionGroups = withdrawOptionGroups?.ToList();
        CreatedByUserId = createdByUserId;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public decimal Value { get; set; }
    public string Endpoint { get; private set; }
    public EndpointContentType ContentType { get; private set; }
    public string EndpointContent { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset DateDeleted { get; private set; }
    public int? CreatedByUserId { get; private set; }

    public int? WithdrawOptionEndpointId { get; private set; }
    public WithdrawOptionEndpoint WithdrawOptionEndpoint { get; private set; }

    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; private set; }
    public ICollection<OutCoin> OutCoins { get; private set; }

    public void Update(
        string title,
        string description,
        string imageUrl,
        decimal value,
        string endpoint = null,
        EndpointContentType? contentType = null,
        string endpointContent = null,
        int? withdrawOptionEndpointId = null,
        IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Value = value;
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

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}