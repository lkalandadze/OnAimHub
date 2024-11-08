using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Level : BaseEntity<int>
{
    public Level(int number, int experienceToArchieve)
    {
        Number = number;
        ExperienceToArchieve = experienceToArchieve;
    }

    public int Number { get; set; }
    public int ExperienceToArchieve { get; set; }
    public int ActId { get; set; }
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
