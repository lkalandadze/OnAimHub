using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetAll;

public sealed class GetAllPromotionViewTemplatesQueryHandler : IQueryHandler<GetAllPromotionViewTemplatesQuery, ApplicationResult>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public GetAllPromotionViewTemplatesQueryHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetAllPromotionViewTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionViewTemplateService.GetAllPromotionViewTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
