namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class TemplatePrizeDto
{
    public int Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    //public string PrizeType { get; set; }
    public int Amount { get; set; }
}
