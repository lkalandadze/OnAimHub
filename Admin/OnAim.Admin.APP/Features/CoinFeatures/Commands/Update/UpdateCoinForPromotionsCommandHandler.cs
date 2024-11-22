using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Commands.Update;

public class UpdateCoinForPromotionsCommandHandler : ICommandHandler<UpdateCoinForPromotionsCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public UpdateCoinForPromotionsCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }
    public async Task<ApplicationResult> Handle(UpdateCoinForPromotionsCommand request, CancellationToken cancellationToken)
    {
        //var result = await _coinService.UpdateCoinForPromotion(request.PromotionIds, request.CoinId, request.UpdatedCoin);

        return new ApplicationResult { Data = "" };
    }
}
