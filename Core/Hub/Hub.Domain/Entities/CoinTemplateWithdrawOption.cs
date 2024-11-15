#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class CoinTemplateWithdrawOption : BaseEntity<int>
{
    public CoinTemplateWithdrawOption()
    {
        
    }

    public CoinTemplateWithdrawOption(int coinTemplateId, int withdrawOptionId)
    {
        CoinTemplateId = coinTemplateId;
        WithdrawOptionId = withdrawOptionId;
    }
    
    public int CoinTemplateId { get; set; }
    public CoinTemplate CoinTemplate { get; set; }

    public int WithdrawOptionId { get; set; }
    public WithdrawOption WithdrawOption { get; set; }
}