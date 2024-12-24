namespace OnAim.Admin.Contracts.Dtos.Coin;

public class UpdateWithdrawOptionGroupDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PriorityIndex { get; set; }
    public List<int> WithdrawOptionIds { get; set; } = new List<int>();
}
