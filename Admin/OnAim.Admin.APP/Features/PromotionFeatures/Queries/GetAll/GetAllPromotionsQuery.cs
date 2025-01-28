using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;

public record GetAllPromotionsQuery(PromotionFilter Filter) : IQuery<ApplicationResult<PaginatedResult<PromotionDto>>>;
