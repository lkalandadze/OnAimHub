using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;

public sealed class GetPromotionTemplateByIdQueryHandler : IQueryHandler<GetPromotionTemplateByIdQuery, ApplicationResult<PromotionTemplateListDto>>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public GetPromotionTemplateByIdQueryHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult<PromotionTemplateListDto>> Handle(GetPromotionTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _promotionTemplateService.GetPromotionTemplateById(request.Id);
    }
}
