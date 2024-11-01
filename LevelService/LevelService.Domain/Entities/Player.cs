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
    public ICollection<PlayerReward> PlayerRewards { get; private set; } = new List<PlayerReward>();

    public void AddExperience(decimal experiencePoints, Stage activeStage)
    {
        Experience += experiencePoints;
        GrantLevelRewardsIfEligible(activeStage);
    }

    private void GrantLevelRewardsIfEligible(Stage activeStage)
    {
        foreach (var level in activeStage.Levels.OrderBy(l => l.Number))
        {
            if (Experience >= level.ExperienceToArchieve &&
                !PlayerRewards.Any(r => r.LevelPrizeId == level.Id))
            {
                foreach (var prize in level.LevelPrizes)
                {
                    var reward = new PlayerReward(Id, prize.Id);
                    PlayerRewards.Add(reward);
                }
            }
        }
    }
}
