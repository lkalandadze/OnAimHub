using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetAll;

public sealed class GetAllPromotionViewTemplatesQueryHandler : IQueryHandler<GetAllPromotionViewTemplatesQuery, ApplicationResult<PaginatedResult<PromotionViewTemplate>>>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public GetAllPromotionViewTemplatesQueryHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult<PaginatedResult<PromotionViewTemplate>>> Handle(GetAllPromotionViewTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _promotionViewTemplateService.GetAllPromotionViewTemplates(request.Filter);
    }
}
