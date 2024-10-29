using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player(int id, string username)
    {
        Id = id;
        Username = username;
    }

    public string Username { get; set; }
}
