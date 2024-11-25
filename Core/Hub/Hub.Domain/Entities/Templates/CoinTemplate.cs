#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities.Templates;

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

    public void Update(string name, string description, string imageUrl, CoinType coinType, IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;

        if (withdrawOptions != null)
        {
            SetWithdrawOptions(withdrawOptions);
        }
    }

    public void SetWithdrawOptions(IEnumerable<WithdrawOption> withdrawOptions)
    {
        if (withdrawOptions != null)
        {
            WithdrawOptions.Clear();

            foreach (var option in withdrawOptions)
            {
                WithdrawOptions.Add(option);
            }
        }
    }

    public void AddWithdrawOptions(IEnumerable<WithdrawOption> withdrawOptions)
    {
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