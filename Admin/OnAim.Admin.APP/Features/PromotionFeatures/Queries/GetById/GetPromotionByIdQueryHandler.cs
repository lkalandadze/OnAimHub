using MongoDB.Bson;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;

public class GetPromotionByIdQueryHandler : IQueryHandler<GetPromotionByIdQuery, ApplicationResult<PromotionDto>>
{
    private readonly IPromotionService _promotionService;

    public GetPromotionByIdQueryHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult<PromotionDto>> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _promotionService.GetPromotionById(request.Id);
    }
}
