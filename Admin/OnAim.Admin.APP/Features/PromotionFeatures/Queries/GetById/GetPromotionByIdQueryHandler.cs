using MongoDB.Bson;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;

public class GetPromotionByIdQueryHandler : IQueryHandler<GetPromotionByIdQuery, ApplicationResult>
{
    private readonly IPromotionService _promotionService;

    public GetPromotionByIdQueryHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionService.GetPromotionById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
