using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;

public record GetLeaderboardRecordByIdQuery(int Id) : IQuery<ApplicationResult<Services.LeaderBoard.LeaderBoardData>>;
