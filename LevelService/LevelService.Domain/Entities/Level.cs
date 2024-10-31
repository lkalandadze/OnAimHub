using LevelService.Domain.Enum;
using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Level : BaseEntity<int>
{
    public Level(int number, int experienceToArchieve)
    {
        Number = number;
        ExperienceToArchieve = experienceToArchieve;
    }

    public int Number { get; private set; }
    public int ExperienceToArchieve { get; private set; }
    public int ActId { get; private set; }
    public ICollection<LevelPrize> LevelPrizes { get; set; }

    public void Update(int number, int experienceToArchieve)
    {
        Number = number;
        ExperienceToArchieve = experienceToArchieve;
    }


    public void UpdateLevelPrizes(int id, int amount, string currencyId, PrizeDeliveryType prizeType)
    {
        var prize = LevelPrizes.FirstOrDefault(x => x.Id == id);

        if (prize == null) return;

        prize.Update(amount, currencyId, prizeType);
    }
}