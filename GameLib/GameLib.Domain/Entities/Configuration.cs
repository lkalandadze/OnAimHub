﻿using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Configuration : BaseEntity<int>
{
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public int GameVersionId { get; set; }
    public GameVersion GameVersion { get; set; }

    //public ICollection<BasePrizeGroup> PrizeGroups { get; set; }
    //public ICollection<BasePrize> Prizes { get; set; }
}