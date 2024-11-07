using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;

public record GetLeaderboardRecordByIdQuery(int Id) : IQuery<ApplicationResult>;
