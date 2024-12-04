namespace OnAim.Admin.Domain.HubEntities.Enum;

public class RewardSource : DbEnum<int, RewardSource>
{
    public static RewardSource Level => FromId(1);
    public static RewardSource Mission => FromId(2);
    public static RewardSource Leaderboard => FromId(3);
}