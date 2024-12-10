using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;

public record GetPromotionTemplateByIdQuery(string Id) : IQuery<ApplicationResult>;
