using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.Domain.HubEntities;

public class PromotionCoin : BaseEntity<string>
{
    public PromotionCoin()
    {

    }

    public PromotionCoin(string id,
        string name,
        string description,
        string imageUrl,
        CoinType coinType,
        int promotionId,
        int? coinTemplateId = null,
        IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
        PromotionId = promotionId;
        CoinTemplateId = coinTemplateId;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public int? CoinTemplateId { get; set; }
    public CoinTemplate CoinTemplate { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
}
