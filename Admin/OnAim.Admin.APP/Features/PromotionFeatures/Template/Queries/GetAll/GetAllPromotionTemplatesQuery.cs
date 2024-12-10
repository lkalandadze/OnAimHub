using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;

public record GetAllPromotionTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult>;
