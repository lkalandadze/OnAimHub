using Hub.Application.Models.Tansaction;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract;

public interface ITransactionService
{
    Task<TransactionResponseModel> CreateTransaction(int gameId, string currencyId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType);
}