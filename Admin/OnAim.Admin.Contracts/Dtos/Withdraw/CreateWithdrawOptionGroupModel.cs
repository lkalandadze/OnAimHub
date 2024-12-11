namespace OnAim.Admin.Contracts.Dtos.Withdraw;

public class CreateWithdrawOptionGroupModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }
}
