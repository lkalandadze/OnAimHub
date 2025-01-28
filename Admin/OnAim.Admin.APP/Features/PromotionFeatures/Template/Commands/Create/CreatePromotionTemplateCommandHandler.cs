using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Create;

public sealed class CreatePromotionTemplateCommandHandler : ICommandHandler<CreatePromotionTemplateCommand, ApplicationResult<bool>>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public CreatePromotionTemplateCommandHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(CreatePromotionTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _promotionTemplateService.CreatePromotionTemplate(request.Create);
    }
}
