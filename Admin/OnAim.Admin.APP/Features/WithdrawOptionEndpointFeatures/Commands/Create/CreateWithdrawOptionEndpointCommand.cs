using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Create;

public record CreateWithdrawOptionEndpointCommand(CreateWithdrawOptionEndpointDto Command) : ICommand<ApplicationResult<object>>;

public sealed class CreateWithdrawOptionEndpointCommandHandler : ICommandHandler<CreateWithdrawOptionEndpointCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionEndpointCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(CreateWithdrawOptionEndpointCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.CreateWithdrawOptionEndpoint(request.Command);
    }
}
