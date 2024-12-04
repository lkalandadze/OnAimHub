using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinService
{
    Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOption option);
    Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOption option);
    Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpoint option);
    Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpoint option);
    Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroup option);
    Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroup option);
}
