using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Create;

public record CreateWithdrawOptionEndpointCommand(CreateWithdrawOptionEndpointDto Command) : ICommand<ApplicationResult>;

public sealed class CreateWithdrawOptionEndpointCommandHandler : ICommandHandler<CreateWithdrawOptionEndpointCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public CreateWithdrawOptionEndpointCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(CreateWithdrawOptionEndpointCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.CreateWithdrawOptionEndpoint(request.Command);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
