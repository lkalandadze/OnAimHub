using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Configuration : BaseEntity<int>
{
    public bool IsCustom { get; set; }
}
