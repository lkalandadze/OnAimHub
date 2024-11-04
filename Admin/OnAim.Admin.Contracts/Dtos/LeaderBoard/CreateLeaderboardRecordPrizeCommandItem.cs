namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardRecordPrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}
