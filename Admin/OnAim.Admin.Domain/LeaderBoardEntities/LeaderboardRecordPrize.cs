namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardRecordPrize : BasePrize
{
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
}
