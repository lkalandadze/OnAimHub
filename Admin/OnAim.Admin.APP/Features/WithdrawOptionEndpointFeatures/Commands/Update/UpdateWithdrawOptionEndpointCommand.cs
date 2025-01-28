using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Update;

public record UpdateWithdrawOptionEndpointCommand(UpdateWithdrawOptionEndpointDto Command) : ICommand<ApplicationResult<object>>;

public sealed class UpdateWithdrawOptionEndpointCommandHandler : ICommandHandler<UpdateWithdrawOptionEndpointCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public UpdateWithdrawOptionEndpointCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(UpdateWithdrawOptionEndpointCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.UpdateWithdrawOptionEndpoint(request.Command);
    }
}
