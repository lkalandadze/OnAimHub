using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player(int id, string username)
    {
        Id = id;
        Username = username;
        Experience = 0;
    }

    public string Username { get; private set; }
    public decimal Experience { get; private set; }

    public void AddExperience(decimal experiencePoints)
    {
        Experience += experiencePoints;
    }
}
