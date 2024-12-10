using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;

public sealed class GetPromotionTemplateByIdQueryHandler : IQueryHandler<GetPromotionTemplateByIdQuery, ApplicationResult>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public GetPromotionTemplateByIdQueryHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetPromotionTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionTemplateService.GetPromotionTemplateById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
