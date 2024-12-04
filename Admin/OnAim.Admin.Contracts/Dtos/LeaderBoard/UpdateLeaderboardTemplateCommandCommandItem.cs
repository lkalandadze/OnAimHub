namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardTemplateCommandCommandItem
{
    public string Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string CoinId { get; set; }
    public int Amount { get; set; }
}
