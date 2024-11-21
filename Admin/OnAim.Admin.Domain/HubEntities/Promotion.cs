namespace OnAim.Admin.Domain.HubEntities;

public class Promotion : BaseEntity<int>
{
    public Promotion()
    {

    }

    public Promotion(
        PromotionStatus status,
        DateTimeOffset startDate,
        DateTimeOffset EndDate,
        string title,
        string description,
        IEnumerable<PromotionCoin> promotionCoins = null)
    {
        Status = status;
        StartDate = startDate;
        this.EndDate = EndDate;
        Title = title;
        Description = description;
        Coins = promotionCoins?.ToList() ?? [];
    }
    public int Id { get; set; }
    public decimal? TotalCost { get; set; }
    public PromotionStatus Status { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<PromotionService> Services { get; set; }
    public ICollection<Segment> Segments { get; set; }
    public ICollection<PromotionCoin> Coins { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}
