using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;

public sealed class GetAllPromotionTemplatesQueryHandler : IQueryHandler<GetAllPromotionTemplatesQuery, ApplicationResult<PaginatedResult<PromotionTemplateListDto>>>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public GetAllPromotionTemplatesQueryHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult<PaginatedResult<PromotionTemplateListDto>>> Handle(GetAllPromotionTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _promotionTemplateService.GetAllPromotionTemplates(request.Filter);
    }
}
