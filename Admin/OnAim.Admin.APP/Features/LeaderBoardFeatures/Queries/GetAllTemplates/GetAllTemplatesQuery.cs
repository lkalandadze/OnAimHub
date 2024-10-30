using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllTemplates;

public record GetAllTemplatesQuery(BaseFilter? Filter) : IQuery<ApplicationResult>;
