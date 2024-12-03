using Hub.Application.Models.Coin;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface ICoinService
{
    Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(CreateOutCoinModel? outCoinModel);

    Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(CreateOutCoinModel? outCoinModel);
}