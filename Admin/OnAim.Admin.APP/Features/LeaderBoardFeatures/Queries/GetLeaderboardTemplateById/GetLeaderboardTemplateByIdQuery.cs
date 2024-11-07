using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardTemplateById;

public record GetLeaderboardTemplateByIdQuery(int Id) : IQuery<ApplicationResult>;
