#nullable disable

using Hub.Domain.Entities.Coins;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionGroup : BaseEntity<int>
{
    public WithdrawOptionGroup()
    {
    }

    public WithdrawOptionGroup(
        string title, 
        string description, 
        string imageUrl,  
        int? priorityIndex = null,
        IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        PriorityIndex = priorityIndex;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public string Title { get; set; }
    public string Description { get; set; }    
    public string ImageUrl { get; set; }
    public int? PriorityIndex { get; set; }

    public ICollection<OutCoin> OutCoins { get; set; }
    public ICollection<WithdrawOption> WithdrawOptions { get; set; }

    public void Update(string title, string description, string imageUrl, int? priorityIndex = null, IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        PriorityIndex = priorityIndex;
        WithdrawOptions = withdrawOptions?.ToList();
    }

    public void AddWithdrawOptions(IEnumerable<WithdrawOption> withdrawOptions)
    {
        if (WithdrawOptions == null)
        {
            WithdrawOptions = [];
        }

        foreach (var withdrawOption in withdrawOptions)
        {
            if (!WithdrawOptions.Contains(withdrawOption))
            {
                WithdrawOptions.Add(withdrawOption);
            }
        }
    }
}