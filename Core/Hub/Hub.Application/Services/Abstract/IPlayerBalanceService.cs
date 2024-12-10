using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Abstract;

public interface IPlayerBalanceService
{
    Task<PlayerBalance> GetOrCreatePlayerBalanceAsync(int playerId, string currencyId, int promotionId);

    Task ApplyPlayerBalanceOperationAsync(int playerId, string currencyId, AccountType fromAccount, AccountType toAccount, decimal amount, int promotionId);

    Task ResetBalancesByCurrencyIdAsync(string currencyId);
}