namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardTemplatePrize : BasePrize
{
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
}
