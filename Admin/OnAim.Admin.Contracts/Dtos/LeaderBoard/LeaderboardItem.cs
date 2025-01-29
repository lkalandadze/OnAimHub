namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderboardItem
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int Placement { get; set; }
    public int Score { get; set; }
    public int? CoinId { get; set; }
    public decimal? PrizeAmount { get; set; }
    public int PromotionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}