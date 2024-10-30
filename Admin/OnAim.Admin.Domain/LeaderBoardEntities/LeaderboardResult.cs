using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardResult : BaseEntity<int>
{
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Placement { get; set; }
    public int Amount { get; set; }
}
public enum JobTypeEnum
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Custom = 3
}
public enum LeaderboardRecordStatus
{
    Future = 0,
    Created = 1,
    Announced = 2,
    InProgress = 3,
    Finished = 4,
}
public enum LeaderboardType
{
    Win = 0
}
