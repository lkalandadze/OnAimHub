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
public class LeaderboardRecord : BaseEntity<int>
{
    public string Name { get; set; }
    public int? LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; } = new List<LeaderboardProgress>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>(); 
}
public class LeaderboardRecordPrize : BasePrize
{
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
}

public class LeaderboardTemplate : BaseEntity<int>
{
    public string Name { get; set; }

    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public ICollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; set; } = new List<LeaderboardTemplatePrize>();
}
public class LeaderboardProgress : BaseEntity<int>
{
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Amount { get; set; }
}
public class LeaderboardTemplatePrize : BasePrize
{
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
}
public class BasePrize : BaseEntity<int>
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public Prize Prize { get; set; }
    public int Amount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}
public class Prize : BaseEntity<string>
{
    public string Name { get; set; }
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
