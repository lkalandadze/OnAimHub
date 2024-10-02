namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerProgress : BaseEntity<Int32>	{
		public Int32 Progress { get; set; }
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public String CurrencyId { get; set; }
		public Currency Currency { get; set; }
	}
}
