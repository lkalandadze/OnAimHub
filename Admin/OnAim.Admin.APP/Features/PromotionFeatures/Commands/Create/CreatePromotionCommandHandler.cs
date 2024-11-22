using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;

public class CreatePromotionCommandHandler : ICommandHandler<CreatePromotionCommand, ApplicationResult>
{
    private readonly IPromotionService _promotionService;

    public CreatePromotionCommandHandler(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    public async Task<ApplicationResult> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        //var result = await _promotionService.CreatePromotion(request.Command);

        return new ApplicationResult { Data = "" };
    }
}
