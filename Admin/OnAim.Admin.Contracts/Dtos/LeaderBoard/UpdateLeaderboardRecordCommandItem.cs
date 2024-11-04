namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardRecordCommandItem
{
    public int Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}