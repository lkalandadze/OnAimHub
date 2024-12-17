using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinService
{
    Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<ApplicationResult> GetAllWithdrawOptions(BaseFilter filter);
    Task<ApplicationResult> GetWithdrawOptionById(int id);
    Task<ApplicationResult> GetAllWithdrawOptionGroups(BaseFilter filter);
    Task<ApplicationResult> GetWithdrawOptionGroupById(int id);
    Task<ApplicationResult> GetWithdrawOptionEndpoints(BaseFilter filter);
    Task<ApplicationResult> GetWithdrawOptionEndpointById(int id);
    Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOption option);
    Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOption option);
    Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpoint option);
    Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpoint option);
    Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroup option);
    Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroup option);
    Task<ApplicationResult> DeleteWithdrawOption(APP.DeleteWithdrawOption delete);
    Task<ApplicationResult> DeleteWithdrawOptionEndpoint(APP.DeleteWithdrawOptionEndpoint delete);
    Task<ApplicationResult> DeleteWithdrawOptiongroup(APP.DeleteWithdrawOptionGroup delete);
}
