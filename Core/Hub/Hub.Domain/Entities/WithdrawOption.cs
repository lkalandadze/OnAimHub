#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOption : BaseEntity<int>
{
    public WithdrawOption()
    {
    }

    public WithdrawOption(string title, string description, string imageUrl, string endpoint = null, string endpointContent = null, int? withdrawEndpointTemplateId = null, IEnumerable<PromotionCoin> promotionCoins = null, IEnumerable<CoinTemplate> coinTemplates = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        FromTemplateId = withdrawEndpointTemplateId;
        PromotionCoins = promotionCoins?.ToList() ?? [];
        CoinTemplates = coinTemplates?.ToList() ?? [];
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public string Endpoint { get; private set; }
    public string EndpointContent { get; private set; }

    public ICollection<PromotionCoin> PromotionCoins { get; private set; }
    public ICollection<CoinTemplate> CoinTemplates { get; private set; }
    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; private set; }

    public int? FromTemplateId { get; private set; }
    public WithdrawEndpointTemplate WithdrawEndpointTemplate { get; private set; }

    public void Update(string title, string description, string imageUrl, string endpoint = null, string endpointContent = null, int? fromTemplateId = null, IEnumerable<PromotionCoin> promotionCoins = null, IEnumerable<CoinTemplate> coinTemplates = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        FromTemplateId = fromTemplateId;
        PromotionCoins = promotionCoins?.ToList() ?? [];
        CoinTemplates = coinTemplates?.ToList() ?? [];
    }

    public void AddPromotionCoins(IEnumerable<PromotionCoin> promotionCoins)
    {
        foreach (var promotionCoin in promotionCoins)
        {
            if (!PromotionCoins.Contains(promotionCoin))
            {
                PromotionCoins.Add(promotionCoin);
            }
        }
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