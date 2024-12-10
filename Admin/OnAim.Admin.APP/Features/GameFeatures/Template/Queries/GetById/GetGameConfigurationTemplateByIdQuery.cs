using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetById;

public record GetGameConfigurationTemplateByIdQuery(string Id) : IQuery<ApplicationResult>;
