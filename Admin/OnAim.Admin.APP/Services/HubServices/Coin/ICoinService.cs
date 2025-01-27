using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.HubEntities;

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
