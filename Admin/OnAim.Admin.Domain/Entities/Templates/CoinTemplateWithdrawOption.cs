using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class CoinTemplateWithdrawOption
{
    public CoinTemplateWithdrawOption()
    {

    }

    public CoinTemplateWithdrawOption(string coinTemplateId, int withdrawOptionId)
    {
        CoinTemplateId = coinTemplateId;
        WithdrawOptionId = withdrawOptionId;
    }
    public int Id { get; set; }
    public string CoinTemplateId { get; set; }
    public CoinTemplate CoinTemplate { get; set; }

    public int WithdrawOptionId { get; set; }
    public WithdrawOption WithdrawOption { get; set; }
}