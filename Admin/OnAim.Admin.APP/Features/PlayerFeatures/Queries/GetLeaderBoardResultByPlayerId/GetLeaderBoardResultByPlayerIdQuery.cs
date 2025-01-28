using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public record GetLeaderBoardResultByPlayerIdQuery(int PlayerId) : IQuery<ApplicationResult<List<LeaderboardResult>>>;
