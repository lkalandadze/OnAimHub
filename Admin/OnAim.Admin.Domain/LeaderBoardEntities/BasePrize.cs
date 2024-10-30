﻿using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

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