namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOptionGroup : BaseEntity<int>
{
    public WithdrawOptionGroup()
    {
    }

    public WithdrawOptionGroup(
        string title,
        string description,
        string imageUrl,
        IEnumerable<PromotionCoin> promotionCoins = null,
        IEnumerable<CoinTemplate> coinTemplates = null,
        IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        PromotionCoins = promotionCoins?.ToList() ?? [];
        CoinTemplates = coinTemplates?.ToList() ?? [];
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
    public ICollection<PromotionCoin> PromotionCoins { get; set; }
    public ICollection<CoinTemplate> CoinTemplates { get; set; }

}
