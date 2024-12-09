using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Create;

public sealed class CreatePromotionTemplateCommandHandler : ICommandHandler<CreatePromotionTemplateCommand, ApplicationResult>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public CreatePromotionTemplateCommandHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult> Handle(CreatePromotionTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _promotionTemplateService.CreatePromotionTemplate(request.Create);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
