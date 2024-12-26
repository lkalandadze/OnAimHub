namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionGameDto
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public string Description { get; set; }
    public int BetPrice { get; set; }
    public string Coins { get; set; }
}
