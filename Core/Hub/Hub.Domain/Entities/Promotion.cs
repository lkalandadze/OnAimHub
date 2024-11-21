using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Promotion : BaseEntity<int>
{
    public Promotion(PromotionStatus status, DateTimeOffset startDate, DateTimeOffset endDate, string title, string description)
    {
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        Title = title;
        Description = description;
        CreateDate = DateTimeOffset.UtcNow;
    }

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

    public void UpdatePrice(decimal? totalCost)
    {
        TotalCost = totalCost;
    }

    public void Update(PromotionStatus status, DateTimeOffset startDate, DateTimeOffset endDate, string title, string description)
    {
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        Title = title;
        Description = description;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
