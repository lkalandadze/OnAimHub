#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class CoinTemplate : BaseEntity<int>
{
    public CoinTemplate()
    {
        
    }

    public CoinTemplate(
        string name, 
        string imageUrl, 
        CoinType coinType,
        IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Name = name;
        ImageUrl = imageUrl;
        CoinType = coinType;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
}