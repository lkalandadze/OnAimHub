using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Create;

public sealed class CreatePromotionViewTemplateCommandHandler : ICommandHandler<CreatePromotionViewTemplateCommand, ApplicationResult<bool>>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public CreatePromotionViewTemplateCommandHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(CreatePromotionViewTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _promotionViewTemplateService.CreatePromotionViewTemplateAsync(request.Create);
    }
}
