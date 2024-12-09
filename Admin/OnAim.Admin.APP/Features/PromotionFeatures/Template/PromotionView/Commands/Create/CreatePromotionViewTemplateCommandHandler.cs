using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Create;

public sealed class CreatePromotionViewTemplateCommandHandler : ICommandHandler<CreatePromotionViewTemplateCommand, ApplicationResult>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public CreatePromotionViewTemplateCommandHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult> Handle(CreatePromotionViewTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _promotionViewTemplateService.CreatePromotionViewTemplateAsync(request.Create);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
