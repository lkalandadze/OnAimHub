using OnAim.Admin.Domain.HubEntities.Coin;

namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOptionGroup
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

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int? PriorityIndex { get; set; }
    public int? CreatedByUserId { get; set; }
    public ICollection<OutCoin> OutCoins { get; set; }
    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
}