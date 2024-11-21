#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;
using System.Linq;

namespace Hub.Domain.Entities;

public class PromotionCoin : BaseEntity<string>
{
    public PromotionCoin()
    {
        
    }

    public PromotionCoin(string id, string name, string imageUrl, int promotionId, CoinType coinType, IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        PromotionId = promotionId;
        CoinType = coinType;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }

    public void Delete()
    { 
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }

    //public void AddWithdrawOptionGroup(IEnumerable<WithdrawOption> withdrawOptionGroups)
    //{
    //    foreach(var withdrawOptionGroup in withdrawOptionGroups)
    //    {
    //        if (!WithdrawOptionGroups.Contains(withdrawOptionGroup))
    //        {
    //            WithdrawOptionGroups.Add(withdrawOptionGroup);
    //        }
    //    }
    //}
    public void AddWithdrawOption(IEnumerable<WithdrawOption> withdrawOptions)
    {
        foreach (var withdrawOption in withdrawOptions)
        {
            if (!WithdrawOptions.Contains(withdrawOption))
            {
                WithdrawOptions.Add(withdrawOption);
            }
        }
    }
}