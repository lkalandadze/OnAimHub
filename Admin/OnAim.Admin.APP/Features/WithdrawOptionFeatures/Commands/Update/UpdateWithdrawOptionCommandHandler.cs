using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Update;

public sealed class UpdateWithdrawOptionCommandHandler : ICommandHandler<UpdateWithdrawOptionCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public UpdateWithdrawOptionCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(UpdateWithdrawOptionCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.UpdateWithdrawOption(request.Command);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
