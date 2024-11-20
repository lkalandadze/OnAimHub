#nullable disable

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
        EndpointContentType contentType,
        string endpoint = null, 
        string endpointContent = null, 
        int? withdrawEndpointTemplateId = null, 
        IEnumerable<CoinTemplate> coinTemplates = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        ContentType = contentType;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = withdrawEndpointTemplateId;
        CoinTemplates = coinTemplates?.ToList() ?? [];
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public ICollection<CoinTemplate> CoinTemplates { get; private set; }
    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; private set; }

    public int? WithdrawEndpointTemplateId { get; private set; }
    public WithdrawEndpointTemplate WithdrawEndpointTemplate { get; private set; }

    public void Update(string title, string description, string imageUrl, string endpoint = null, string endpointContent = null, int? withdrawEndpointTemplateId = null, IEnumerable<PromotionCoin> promotionCoins = null, IEnumerable<CoinTemplate> coinTemplates = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        WithdrawEndpointTemplateId = withdrawEndpointTemplateId;
        CoinTemplates = coinTemplates?.ToList() ?? [];
    }

    public void AddCoinTemplates(IEnumerable<CoinTemplate> coinTemplates)
    {
        foreach (var coinTemplate in coinTemplates)
        {
            if (!CoinTemplates.Contains(coinTemplate))
            {
                CoinTemplates.Add(coinTemplate);
            }
        }
    }
}