using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;

public record GetAllLeaderBoardQuery(LeaderBoardFilter? Filter) : IQuery<ApplicationResult<PaginatedResult<LeaderBoardListDto>>>;
