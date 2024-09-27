using Hub.Application.Models.Tansaction;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Abstract;

public interface ITransactionService
{
    Task<TransactionResponseModel> CreateTransactionAndApplyBalanceAsync(int? gameId, string currencyId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType);
}