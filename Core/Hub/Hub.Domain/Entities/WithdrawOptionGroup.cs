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

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public int? PriorityIndex { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset DateDeleted { get; private set; }

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

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}