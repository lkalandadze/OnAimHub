#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Game : BaseEntity<int>
{
    public Game()
    {

    }

    public Game(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public void ChangeDetails(string name)
    {
        Name = name;
    }
}