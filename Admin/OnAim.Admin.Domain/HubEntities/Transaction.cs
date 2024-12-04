using OnAim.Admin.Domain.HubEntities.Enum;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;

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
		public string CurrencyId { get; set; }
		public Currency Currency { get; set; }
		public Int32 StatusId { get; set; }
		public TransactionStatus Status { get; set; }
		public Int32 TypeId { get; set; }
		public TransactionType Type { get; set; }
	}
    public class AccountType : DbEnum<int, AccountType>
    {
        public static AccountType Player => FromId(1);
        public static AccountType Game => FromId(2);
        public static AccountType Casino => FromId(3);
        public static AccountType Reset => FromId(4);
    }
}
