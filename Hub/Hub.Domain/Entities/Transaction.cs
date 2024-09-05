using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Transaction : BaseEntity<int>
{
    public decimal Amount { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; }

    public int FromAccountId { get; set; }
    public AccountType FromAccount { get; set; }

    public int ToAccountId { get; set; }
    public AccountType ToAccount { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public int StatusId { get; set; }
    public TransactionStatus Status { get; set; }

    public int TypeId { get; set; }
    public TransactionType Type { get; set; }
}