namespace OnAim.Admin.Domain.HubEntities
{
	// Generated Code

	public class ReferralDistribution : BaseEntity<Int32>	{
		public Int32 ReferrerId { get; set; }
		public Int32 ReferralId { get; set; }
		public Int32 ReferrerPrizeValue { get; set; }
		public String ReferrerPrizeId { get; set; }
		public Currency ReferrerPrizeCurrency { get; set; }
		public Int32 ReferralPrizeValue { get; set; }
		public String ReferralPrizeId { get; set; }
		public Currency ReferralPrizeCurrency { get; set; }
		public Player Referrer { get; set; }
		public Player Referral { get; set; }
		public DateTimeOffset DateCreated { get; set; }
	}
}
