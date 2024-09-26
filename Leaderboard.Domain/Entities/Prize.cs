using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Prize : BaseEntity<string>
{
    public string Name { get; set; }
}
