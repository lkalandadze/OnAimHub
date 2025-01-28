using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinService
{
    Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel);
    Task<ApplicationResult<PaginatedResult<WithdrawOptionDto>>> GetAllWithdrawOptions(BaseFilter filter);
    Task<ApplicationResult<WithdrawOptionDto>> GetWithdrawOptionById(int id);
    Task<ApplicationResult<PaginatedResult<WithdrawOptionGroupDto>>> GetAllWithdrawOptionGroups(BaseFilter filter);
    Task<ApplicationResult<WithdrawOptionGroupDto>> GetWithdrawOptionGroupById(int id);
    Task<ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>> GetWithdrawOptionEndpoints(BaseFilter filter);
    Task<ApplicationResult<WithdrawOptionEndpointDto>> GetWithdrawOptionEndpointById(int id);
    Task<ApplicationResult<object>> CreateWithdrawOption(CreateWithdrawOptionDto option);
    Task<ApplicationResult<object>> UpdateWithdrawOption(UpdateWithdrawOptionDto option);
    Task<ApplicationResult<object>> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpointDto option);
    Task<ApplicationResult<object>> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpointDto option);
    Task<ApplicationResult<object>> CreateWithdrawOptionGroup(CreateWithdrawOptionGroupDto option);
    Task<ApplicationResult<object>> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroupDto option);
    Task<ApplicationResult<object>> DeleteWithdrawOption(List<int> id);
    Task<ApplicationResult<object>> DeleteWithdrawOptionEndpoint(List<int> id);
    Task<ApplicationResult<object>> DeleteWithdrawOptiongroup(List<int> id);
}
