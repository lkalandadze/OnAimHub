namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardItemsDto
{
    public int PlayerId { get; set; }
    public string UserName { get; set; }
    public string Segment {  get; set; }
    public int Place {  get; set; }
    public decimal Score { get; set; }
    public string PrizeType { get; set; }
    public string PrizeValue { get; set; }

}
