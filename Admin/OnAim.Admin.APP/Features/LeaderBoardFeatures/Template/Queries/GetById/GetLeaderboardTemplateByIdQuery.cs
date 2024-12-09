using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetById;

public record GetLeaderboardTemplateByIdQuery(string Id) : IQuery<ApplicationResult>;
