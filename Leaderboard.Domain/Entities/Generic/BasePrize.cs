﻿using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities.Generic;

public class BasePrize : BaseEntity<int>
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public Prize Prize { get; set; }
    public int Amount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}