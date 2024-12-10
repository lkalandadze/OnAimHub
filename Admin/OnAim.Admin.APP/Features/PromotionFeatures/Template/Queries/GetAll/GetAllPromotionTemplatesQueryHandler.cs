using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;

public sealed class GetAllPromotionTemplatesQueryHandler : IQueryHandler<GetAllPromotionTemplatesQuery, ApplicationResult>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public GetAllPromotionTemplatesQueryHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetAllPromotionTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _promotionTemplateService.GetAllPromotionTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
