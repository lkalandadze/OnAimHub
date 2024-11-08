using LevelService.Application.Models.LevelPrizes;
using LevelService.Application.Models.Levels;
using LevelService.Domain.Entities;

namespace LevelService.Application.Features.LevelFeatures.Helper;

public static class LevelModelExtensions
{
    public static LevelModel MapFrom(Level level)
    {
        return new LevelModel
        {
            Number = level.Number,
            ExperienceToArchive = level.ExperienceToArchieve,
            Prizes = level.LevelPrizes.Select(prize => new LevelPrizesModel
            {
                Amount = prize.Amount,
                PrizeTypeId = prize.PrizeTypeId,
                PrizeDeliveryType = prize.PrizeDeliveryType
            }).ToList()
        };
    }
}