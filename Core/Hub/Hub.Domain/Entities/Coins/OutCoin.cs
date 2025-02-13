﻿#nullable disable

using Hub.Domain.Enum;

namespace Hub.Domain.Entities.Coins;

public class OutCoin : Coin
{
    public OutCoin()
    {
        
    }

    public OutCoin(string id, string name, string description, string imageUrl, int promotionId, decimal value, string templateId = null, IEnumerable<WithdrawOption> withdrawOptions = null, IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null) 
        : base(id, name, description, imageUrl, CoinType.Out, promotionId, templateId)
    {
        Value = value;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
        WithdrawOptionGroups = withdrawOptionGroups?.ToList() ?? [];
    }

    public decimal Value { get; private set; }
    public ICollection<WithdrawOption> WithdrawOptions { get; private set; }
    public ICollection<WithdrawOptionGroup> WithdrawOptionGroups { get; private set; }

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