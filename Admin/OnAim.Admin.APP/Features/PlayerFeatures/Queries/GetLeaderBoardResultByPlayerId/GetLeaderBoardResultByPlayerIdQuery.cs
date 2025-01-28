using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public record GetLeaderBoardResultByPlayerIdQuery(int PlayerId) : IQuery<ApplicationResult<object>>;
