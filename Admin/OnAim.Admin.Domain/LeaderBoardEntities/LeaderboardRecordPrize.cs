﻿namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardRecordPrize : BasePrize
{
    public LeaderboardRecordPrize(int id, int startRank, int endRank, string coinId, int amount)
    {
        Id = id;
        StartRank = startRank;
        EndRank = endRank;
        CoinId = coinId;
        Amount = amount;
    }
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
}
