using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Delete;

public record DeleteWithdrawOptionEndpointCommand(List<int> Id) : ICommand<ApplicationResult>;

public sealed class DeleteWithdrawOptionEndpointCommandHandler : ICommandHandler<DeleteWithdrawOptionEndpointCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public DeleteWithdrawOptionEndpointCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(DeleteWithdrawOptionEndpointCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.DeleteWithdrawOptionEndpoint(request.Id);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
