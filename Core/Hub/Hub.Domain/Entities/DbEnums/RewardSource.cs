using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class RewardSource : DbEnum<int, RewardSource>
{
    public static RewardSource Level => FromId(1, nameof(Level));
    public static RewardSource Mission => FromId(2, nameof(Mission));
    public static RewardSource Leaderboard => FromId(3, nameof(Leaderboard));
}