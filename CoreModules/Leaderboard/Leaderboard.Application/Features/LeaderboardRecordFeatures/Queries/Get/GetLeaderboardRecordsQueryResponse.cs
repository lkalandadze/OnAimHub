using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;

public class GetLeaderboardRecordsQueryResponse : Response<PagedResponse<LeaderboardRecordsModel>>;