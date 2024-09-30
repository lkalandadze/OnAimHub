namespace NMSPC
{
	// Generated Code
	public class PlayerBalance : BaseEntity<Int32>	{
		public Decimal Amount { get; set; }
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public String CurrencyId { get; set; }
		public Currency Currency { get; set; }
	}
}