using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetById;

public record GetPromotionViewTemplateByIdQuery(string Id) : IQuery<ApplicationResult<PromotionViewTemplate>>;
