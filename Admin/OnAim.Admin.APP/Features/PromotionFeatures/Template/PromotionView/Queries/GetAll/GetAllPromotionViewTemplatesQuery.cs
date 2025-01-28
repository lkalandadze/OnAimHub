using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetAll;

public record GetAllPromotionViewTemplatesQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<PromotionViewTemplate>>>;
