﻿using Hub.Domain.Entities;
using Hub.Domain.Enum;

namespace Hub.Application.Features.LevelFeatures.DataModels;

public sealed record GetLevelsModel
{
    public int Id { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public bool IsCustom { get; set; }
    public ActStatus Status { get; set; }
    public List<GetLevels> Levels { get; set; }

    public static GetLevelsModel MapFrom(Act act)
    {
        return new GetLevelsModel
        {
            Id = act.Id,
            DateFrom = act.DateFrom,
            DateTo = act.DateTo,
            IsCustom = act.IsCustom,
            Status = act.Status,

            Levels = act.Levels?.Select(level => new GetLevels
            {
                Id = level.Id,
                Number = level.Number,
                ExperienceToArchive = level.ExperienceToArchieve,
                Prize = level.LevelPrizes?.Select(prize => new GetLevelPrizes
                {
                    Id = prize.Id,
                    Amount = prize.Amount,
                    PrizeTypeId = prize.PrizeTypeId,
                    PrizeDeliveryType = prize.PrizeDeliveryType
                }).ToList()
            }).ToList()
        };
    }

    public class GetLevels
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ExperienceToArchive { get; set; }
        public List<GetLevelPrizes> Prize { get; set; }
    }

    public class GetLevelPrizes
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string PrizeTypeId { get; set; }
        public PrizeDeliveryType PrizeDeliveryType { get; set; }
    }
}