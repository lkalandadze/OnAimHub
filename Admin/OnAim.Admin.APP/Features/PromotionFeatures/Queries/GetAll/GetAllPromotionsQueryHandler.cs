using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;

public class GetAllPromotionsQueryHandler : IQueryHandler<GetAllPromotionsQuery, ApplicationResult>
{
    private readonly IPromotionService _promotionService;

    public GetAllPromotionsQueryHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionService.GetAllPromotions(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
