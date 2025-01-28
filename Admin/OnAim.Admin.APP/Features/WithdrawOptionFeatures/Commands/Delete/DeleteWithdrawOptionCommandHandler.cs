using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Delete;

public sealed class DeleteWithdrawOptionCommandHandler : ICommandHandler<DeleteWithdrawOptionCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public DeleteWithdrawOptionCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(DeleteWithdrawOptionCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.DeleteWithdrawOption(request.Id);
    }
}
