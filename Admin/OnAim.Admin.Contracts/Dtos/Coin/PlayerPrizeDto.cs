namespace OnAim.Admin.Contracts.Dtos.Coin;

public class PlayerPrizeDto
{
    public bool IsClaimableByPlayer { get; set; }
    public int PlayerId { get; set; }
    public int SourceId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<PrizeDto> Prizes { get; set; }
}