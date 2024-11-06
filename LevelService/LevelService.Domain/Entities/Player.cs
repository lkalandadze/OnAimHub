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
        var existingRewardIds = PlayerRewards.Select(r => r.LevelPrizeId).ToHashSet();

        foreach (var level in activeStage.Levels.OrderBy(l => l.Number))
        {
            // Check if the player meets the experience threshold and doesn't have rewards for this level
            if (Experience >= level.ExperienceToArchieve && !existingRewardIds.Contains(level.Id))
            {
                foreach (var prize in level.LevelPrizes)
                {
                    // Only add reward if it hasn't been granted for this level prize
                    if (!existingRewardIds.Contains(prize.Id))
                    {
                        var reward = new PlayerReward(Id, prize.Id);
                        PlayerRewards.Add(reward);
                        existingRewardIds.Add(prize.Id); // Update cache to include this prize
                    }
                }
            }
        }
    }
}