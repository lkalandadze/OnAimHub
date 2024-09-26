using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Currency : BaseEntity<string>
{
    public Currency(string name) 
    {
        Name = name;
    }
    public string Name { get; set; }
}
