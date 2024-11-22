namespace OnAim.Admin.Domain.HubEntities;

public class CoinTemplate : BaseEntity<int>
{
    public CoinTemplate()
    {

    }

    public CoinTemplate(string name, string description, string imageUrl, CoinType coinType, IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Name { get; private set; }
    public string Description { get; set; }
    public string ImageUrl { get; private set; }
    public CoinType CoinType { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; private set; }
}
