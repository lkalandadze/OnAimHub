using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetById;

public sealed class GetPromotionViewTemplateByIdQueryHandler : IQueryHandler<GetPromotionViewTemplateByIdQuery, ApplicationResult<PromotionViewTemplate>>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public GetPromotionViewTemplateByIdQueryHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult<PromotionViewTemplate>> Handle(GetPromotionViewTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _promotionViewTemplateService.GetPromotionViewTemplateById(request.Id);
    }
}
