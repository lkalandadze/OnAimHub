namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardRecordPrizeCommandItem2
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string Coin { get; set; }
    public int Amount { get; set; }
}