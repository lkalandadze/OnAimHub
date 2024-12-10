using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Delete;

public sealed class DeletePromotionViewTemplateCommandHandler : ICommandHandler<DeletePromotionViewTemplateCommand, ApplicationResult>
{
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;

    public DeletePromotionViewTemplateCommandHandler(IPromotionViewTemplateService promotionViewTemplateService)
    {
        _promotionViewTemplateService = promotionViewTemplateService;
    }

    public async Task<ApplicationResult> Handle(DeletePromotionViewTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _promotionViewTemplateService.DeletePromotionViewTemplate(request.Id);

        return new ApplicationResult { Data = result.Data , Success = result.Success };
    }
}
