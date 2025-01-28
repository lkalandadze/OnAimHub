using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Update;

public sealed class UpdateWithdrawOptionCommandHandler : ICommandHandler<UpdateWithdrawOptionCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public UpdateWithdrawOptionCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(UpdateWithdrawOptionCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.UpdateWithdrawOption(request.Command);
    }
}
