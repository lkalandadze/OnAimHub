﻿#nullable disable

using Hub.Domain.Enum;

namespace Hub.Domain.Entities.PromotionCoins;

public class PromotionOutgoingCoin : PromotionCoin
{
    public PromotionOutgoingCoin()
    {
        
    }

    public PromotionOutgoingCoin(string id, string name, string description, string imageUrl, int promotionId, IEnumerable<WithdrawOption> withdrawOptions = null) 
        : base(id, name, description, imageUrl, CoinType.Outgoing, promotionId)
    {
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
    }

    public ICollection<WithdrawOption> WithdrawOptions { get; private set; }

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