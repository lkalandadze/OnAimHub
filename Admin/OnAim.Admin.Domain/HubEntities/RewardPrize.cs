﻿using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.HubEntities.Enum;

public class RewardPrize : BaseEntity<int>
{
    public RewardPrize()
    {

    }

    public RewardPrize(int value, string prizeTypeId)
    {
        Value = value;
        PrizeTypeId = prizeTypeId;
    }

    public int Value { get; private set; }

    public int RewardId { get; private set; }
    public Reward Reward { get; private set; }

    public string PrizeTypeId { get; private set; }
    public PrizeType PrizeType { get; private set; }
}
