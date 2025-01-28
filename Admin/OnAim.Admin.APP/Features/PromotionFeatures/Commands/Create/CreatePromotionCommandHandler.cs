using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public class CreatePromotionCommandHandler : ICommandHandler<CreatePromotionCommand, ApplicationResult<Guid>>
{
    private readonly IPromotionService _promotionService;

    public CreatePromotionCommandHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult<Guid>> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        return await _promotionService.CreatePromotion(request.Create);
    }
}
