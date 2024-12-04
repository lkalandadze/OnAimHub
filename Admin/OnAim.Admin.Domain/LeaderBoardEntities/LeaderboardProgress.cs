namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardProgress
{
    public int Id { get; set; }
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Amount { get; set; }
}
