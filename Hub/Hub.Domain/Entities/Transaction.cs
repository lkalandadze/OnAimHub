#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Transaction : BaseEntity<int>
{
    public Transaction()
    {
        
    }

    public Transaction(decimal amount, int gameId, int playerId, AccountType fromAccountId, AccountType toAccountId, string currencyId, TransactionStatus status, TransactionType type)
    {
        Amount = amount;
        GameId = gameId;
        PlayerId = playerId;
        FromAccount = fromAccountId;
        ToAccount = toAccountId;
        CurrencyId = currencyId;
        Status = status;
        Type = type;
    }

    public decimal Amount { get; private set; }

    public int GameId { get; private set; }
    public Game Game { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public int FromAccountId { get; private set; }
    public AccountType FromAccount { get; private set; }

    public int ToAccountId { get; private set; }
    public AccountType ToAccount { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public int StatusId { get; private set; }
    public TransactionStatus Status { get; private set; }

    public int TypeId { get; private set; }
    public TransactionType Type { get; private set; }

    public void SetStatus(TransactionStatus status)
    {
        Status = status;
    }
}