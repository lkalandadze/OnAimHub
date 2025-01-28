using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetById;

public record GetLeaderboardTemplateByIdQuery(string Id) : IQuery<ApplicationResult<LeaderBoardTemplateListDto>>;
