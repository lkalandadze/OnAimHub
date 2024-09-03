using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Game : BaseEntity<int>
{
    public string Name { get; set; }
}