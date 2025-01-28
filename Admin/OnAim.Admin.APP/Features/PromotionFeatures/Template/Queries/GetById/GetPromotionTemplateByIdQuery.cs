using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;

public record GetPromotionTemplateByIdQuery(string Id) : IQuery<ApplicationResult<PromotionTemplateListDto>>;
