using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetAll;

public record GetAllLeaderboardTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>>;
