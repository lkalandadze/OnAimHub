using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;

public record GetAllPromotionTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<PromotionTemplateListDto>>>;
