using MongoDB.Bson.Serialization.Attributes;

namespace OnAim.Admin.Domain.HubEntities.Coin;

[BsonDiscriminator("OutCoin")]
public class OutCoin : Coin
{
    public OutCoin()
    {

    }

    public OutCoin(string id, string name, string description, string imageUrl, int promotionId, string? templateId = null, IEnumerable<WithdrawOption> withdrawOptions = null, IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null)
        : base(id, name, description, imageUrl, Domain.HubEntities.Enum.CoinType.Out, promotionId, templateId)
    {
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
        WithdrawOptionGroups = withdrawOptionGroups?.ToList() ?? [];
    }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; set; }

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

    public void AddWithdrawOptionGroups(IEnumerable<WithdrawOptionGroup> withdrawOptionGroups)
    {
        foreach (var withdrawOptionGroup in withdrawOptionGroups)
        {
            if (!WithdrawOptionGroups.Contains(withdrawOptionGroup))
            {
                WithdrawOptionGroups.Add(withdrawOptionGroup);
            }
        }
    }
}