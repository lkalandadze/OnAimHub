using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Delete;

public record DeleteWithdrawOptiongroupCommand(List<int> Id) : ICommand<ApplicationResult>;

public sealed class DeleteWithdrawOptiongroupCommandHandler : ICommandHandler<DeleteWithdrawOptiongroupCommand, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public DeleteWithdrawOptiongroupCommandHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(DeleteWithdrawOptiongroupCommand request, CancellationToken cancellationToken)
    {
        var res = await _coinService.DeleteWithdrawOptiongroup(request.Id);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
