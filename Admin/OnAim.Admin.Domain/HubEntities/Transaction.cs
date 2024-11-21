namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class Transaction : BaseEntity<Int32>	{
        public Transaction()
        {

        }

        public Transaction(decimal amount, int? gameId, int playerId, AccountType fromAccount, AccountType toAccount, string currencyId, TransactionStatus status, TransactionType type, int? promotionId)
        {
            Amount = amount;
            GameId = gameId;
            PlayerId = playerId;
            FromAccountId = fromAccount.Id;
            ToAccountId = toAccount.Id;
            CurrencyId = currencyId;
            StatusId = status.Id;
            TypeId = type.Id;
            PromotionId = promotionId;
        }

        public decimal Amount { get; private set; }

        public int? GameId { get; private set; }
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

        public int? PromotionId { get; private set; }
        public Promotion Promotion { get; private set; }
    }
    public class AccountType : DbEnum<int, AccountType>
    {
        public static AccountType Player => FromId(1);
        public static AccountType Game => FromId(2);
        public static AccountType Casino => FromId(3);
        public static AccountType Reset => FromId(4);
    }
}
