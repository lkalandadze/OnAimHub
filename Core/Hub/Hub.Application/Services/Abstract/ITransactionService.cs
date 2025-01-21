using Hub.Application.Models.Tansaction;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Abstract;

public interface ITransactionService
{
    Task<TransactionResponseModel> CreateTransactionAndApplyBalanceAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId);
    Task<TransactionResponseModel> CreateLeaderboardTransactionAndApplyBalanceAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId, int playerId);
    Task<TransactionResponseModel> CreateTransactionWithEventAsync(int? keyId, string sourceServiceName, string coinId, decimal amount, AccountType fromAccount, AccountType toAccount, TransactionType transactionType, int promotionId, string eventDetails);
}