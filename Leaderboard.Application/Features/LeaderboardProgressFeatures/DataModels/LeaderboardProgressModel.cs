﻿using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;

public sealed record LeaderboardProgressModel
{
    public int Id { get; set; }
    public int LeaderboardRecordId { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Amount { get; set; }


    public static LeaderboardProgressModel MapFrom(LeaderboardProgress leaderboardProgress)
    {
        return new LeaderboardProgressModel
        {
            Id = leaderboardProgress.Id,
            LeaderboardRecordId = leaderboardProgress.LeaderboardRecordId,
            PlayerId = leaderboardProgress.PlayerId,
            PlayerUsername = leaderboardProgress.PlayerUsername,
            Amount = leaderboardProgress.Amount
        };
    }
}