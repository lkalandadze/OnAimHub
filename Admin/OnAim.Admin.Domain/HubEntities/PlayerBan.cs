namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class PlayerBan : BaseEntity<Int32>	{
		public Int32 PlayerId { get; set; }
		public Player Player { get; set; }
		public DateTimeOffset DateBanned { get; set; }
		public Nullable<DateTimeOffset> ExpireDate { get; set; }
		public Boolean IsPermanent { get; set; }
		public Boolean IsRevoked { get; set; }
		public Nullable<DateTimeOffset> RevokeDate { get; set; }
		public String Description { get; set; }
	}
}
