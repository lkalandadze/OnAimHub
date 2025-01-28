using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Delete;

public record DeleteWithdrawOptionEndpointCommand(List<int> Id) : ICommand<ApplicationResult<object>>;

public sealed class DeleteWithdrawOptionEndpointCommandHandler : ICommandHandler<DeleteWithdrawOptionEndpointCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public DeleteWithdrawOptionEndpointCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(DeleteWithdrawOptionEndpointCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.DeleteWithdrawOptionEndpoint(request.Id);
    }
}
