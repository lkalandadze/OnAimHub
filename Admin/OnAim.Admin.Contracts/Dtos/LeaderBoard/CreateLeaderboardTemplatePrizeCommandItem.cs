namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardTemplatePrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string CoinId { get; set; }
    public int Amount { get; set; }
}
