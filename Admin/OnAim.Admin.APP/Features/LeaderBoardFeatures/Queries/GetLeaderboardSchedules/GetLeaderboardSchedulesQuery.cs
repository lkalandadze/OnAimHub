using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;

public record GetLeaderboardSchedulesQuery(int? PageNumber, int? PageSize) : IQuery<ApplicationResult>;
