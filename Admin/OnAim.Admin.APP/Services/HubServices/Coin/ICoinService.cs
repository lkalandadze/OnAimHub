using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
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
    Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOptionDto option);
    Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOptionDto option);
    Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpointDto option);
    Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpointDto option);
    Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroupDto option);
    Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroupDto option);
    Task<ApplicationResult> DeleteWithdrawOption(List<int> id);
    Task<ApplicationResult> DeleteWithdrawOptionEndpoint(List<int> id);
    Task<ApplicationResult> DeleteWithdrawOptiongroup(List<int> id);
}
