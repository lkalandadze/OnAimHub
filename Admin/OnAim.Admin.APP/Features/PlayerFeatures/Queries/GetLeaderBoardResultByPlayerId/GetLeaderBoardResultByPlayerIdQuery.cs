using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Hub.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public record GetLeaderBoardResultByPlayerIdQuery(int PlayerId) : IQuery<UserActiveLeaderboards>;
