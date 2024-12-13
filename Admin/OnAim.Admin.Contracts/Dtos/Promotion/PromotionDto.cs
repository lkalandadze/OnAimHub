using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PromotionStatus Status { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<string> Segments { get; set; }
    //public ICollection<PromotionServiceDto> Services { get; set; }

    public List<GamePromotionDto> Games { get; set; }

    public ICollection<PromotionCoinDto> PromotionCoins { get; set; }
    public ICollection<TransactionDto> Transactions { get; set; }
}
public class GamePromotionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BetPrice { get; set; }
    public decimal Coins { get; set; }
}