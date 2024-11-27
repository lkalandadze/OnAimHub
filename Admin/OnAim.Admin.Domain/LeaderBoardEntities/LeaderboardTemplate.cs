using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardTemplate : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public System.TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public Guid CorrelationId { get; set; }
    public ICollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; set; } = new List<LeaderboardTemplatePrize>();
}
