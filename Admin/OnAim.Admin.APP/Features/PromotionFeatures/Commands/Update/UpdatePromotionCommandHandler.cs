using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Update;

public class UpdatePromotionCommandHandler : ICommandHandler<UpdatePromotionCommand, ApplicationResult>
{
    private readonly IPromotionService _promotionService;

    public UpdatePromotionCommandHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        //var result = await _promotionService.UpdatePromotion(request.Command.promotionId, request.Command.updatedPromotion);

        return new ApplicationResult { Data = "" };
    }
}
