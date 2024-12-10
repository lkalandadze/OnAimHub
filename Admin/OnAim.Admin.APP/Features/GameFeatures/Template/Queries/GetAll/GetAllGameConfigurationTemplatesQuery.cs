using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;

public record GetAllGameConfigurationTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult>;
