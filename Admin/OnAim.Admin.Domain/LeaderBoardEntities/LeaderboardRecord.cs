using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

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
