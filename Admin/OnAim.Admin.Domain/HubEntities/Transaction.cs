namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class Transaction : BaseEntity<Int32>	{
		public Decimal Amount { get; set; }
		public Nullable<Int32> GameId { get; set; }
		public Game Game { get; set; }
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public Int32 FromAccountId { get; set; }
		public AccountType FromAccount { get; set; }
		public Int32 ToAccountId { get; set; }
		public AccountType ToAccount { get; set; }
		public String CurrencyId { get; set; }
		public Currency Currency { get; set; }
		public Int32 StatusId { get; set; }
		public TransactionStatus Status { get; set; }
		public Int32 TypeId { get; set; }
		public TransactionType Type { get; set; }
	}
}
