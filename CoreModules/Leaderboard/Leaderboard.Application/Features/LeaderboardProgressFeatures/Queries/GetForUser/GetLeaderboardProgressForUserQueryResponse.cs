﻿using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQueryResponse : Response<PagedResponse<LeaderboardProgressModel>>
{
    public string? CurrentPlayerUsername { get; set; }
    public int CurrentPlacement { get; set; }
};