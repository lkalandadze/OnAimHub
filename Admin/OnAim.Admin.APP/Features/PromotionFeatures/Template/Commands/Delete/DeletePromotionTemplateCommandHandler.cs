using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Delete;

public sealed class DeletePromotionTemplateCommandHandler : ICommandHandler<DeletePromotionTemplateCommand, ApplicationResult>
{
    private readonly IPromotionTemplateService _promotionTemplateService;

    public DeletePromotionTemplateCommandHandler(IPromotionTemplateService promotionTemplateService)
    {
        _promotionTemplateService = promotionTemplateService;
    }

    public async Task<ApplicationResult> Handle(DeletePromotionTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _promotionTemplateService.DeletePromotionTemplate(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
