using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Delete;

public record DeleteWithdrawOptiongroupCommand(List<int> Id) : ICommand<ApplicationResult<object>>;

public sealed class DeleteWithdrawOptiongroupCommandHandler : ICommandHandler<DeleteWithdrawOptiongroupCommand, ApplicationResult<object>>
{
    private readonly ICoinService _coinService;

    public DeleteWithdrawOptiongroupCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<object>> Handle(DeleteWithdrawOptiongroupCommand request, CancellationToken cancellationToken)
    {
        return await _coinService.DeleteWithdrawOptiongroup(request.Id);
    }
}
