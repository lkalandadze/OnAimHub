namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionLeaderboardDetailDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string UserName { get; set; }
    public string Segment { get; set; }
    public int Place { set; get; }
    public decimal Score { get; set; }
    public string PrizeType { get; set; }
    public int PrizeValue { get; set; }
}
