using Hub.Application.Models.Withdraw.WithdrawOption;

namespace Hub.Application.Models.Withdraw.WithdrawOptionGroup;

public class CreateWithdrawOptionGroupModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }
}
