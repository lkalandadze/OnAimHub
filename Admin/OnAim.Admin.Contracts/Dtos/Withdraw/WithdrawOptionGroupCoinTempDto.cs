namespace OnAim.Admin.Contracts.Dtos.Withdraw;

public class WithdrawOptionGroupCoinTempDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int? PriorityIndex { get; set; }
}