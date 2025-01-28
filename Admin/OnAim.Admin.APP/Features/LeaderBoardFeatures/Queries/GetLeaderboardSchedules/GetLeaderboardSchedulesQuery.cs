using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;

public record GetLeaderboardSchedulesQuery(int? PageNumber, int? PageSize) : IQuery<object>;
