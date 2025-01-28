﻿using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Admin.GetPlayerActiveLeaderboards;

public class GetPlayerActiveLeaderboardsQueryResponse : Response<PagedResponse<UserLeaderboardProgressModel>>;