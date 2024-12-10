using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetById;

public sealed class GetPromotionViewTemplateByIdQueryHandler : IQueryHandler<GetPromotionViewTemplateByIdQuery, ApplicationResult>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public GetPromotionViewTemplateByIdQueryHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetPromotionViewTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionViewTemplateService.GetPromotionViewTemplateById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
