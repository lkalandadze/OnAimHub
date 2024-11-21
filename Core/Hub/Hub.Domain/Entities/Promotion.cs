#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Promotion : BaseEntity<int>
{
    public Promotion()
    {
        
    }

    public Promotion(
        PromotionStatus status, 
        DateTimeOffset startDate, 
        DateTimeOffset endDate, 
        string title, 
        string description,
        IEnumerable<PromotionService> services = null,
        IEnumerable<Segment> segments = null,
        IEnumerable<PromotionCoin> coins = null,
        IEnumerable<PromotionView> views = null)
    {
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
        Title = title;
        Description = description;
        CreateDate = DateTimeOffset.UtcNow;
        Services = services?.ToList() ?? [];
        Segments = segments?.ToList() ?? [];
        Coins = coins?.ToList() ?? [];
        Views = views?.ToList() ?? [];
    }

    public decimal? TotalCost { get; private set; }
    public PromotionStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }
    public bool IsDeleted { get; private set; }

    public ICollection<PromotionService> Services { get; private set; }
    public ICollection<Segment> Segments { get; private set; }
    public ICollection<PromotionCoin> Coins { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; }
    public ICollection<PromotionView> Views { get; private set; }

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
