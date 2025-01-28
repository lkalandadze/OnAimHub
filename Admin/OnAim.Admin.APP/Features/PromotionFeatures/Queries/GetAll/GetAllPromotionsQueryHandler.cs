using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;

public class GetAllPromotionsQueryHandler : IQueryHandler<GetAllPromotionsQuery, ApplicationResult<PaginatedResult<PromotionDto>>>
{
    private readonly IPromotionService _promotionService;

    public GetAllPromotionsQueryHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult<PaginatedResult<PromotionDto>>> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
    {
        return await _promotionService.GetAllPromotions(request.Filter);
    }
}
