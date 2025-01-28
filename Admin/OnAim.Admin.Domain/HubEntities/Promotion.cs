using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities;

public class Promotion
{
    public Promotion()
    {

    }

    public Promotion(
          DateTimeOffset startDate,
          DateTimeOffset endDate,
          string title,
          string description,
          //Guid correlationId,
          IEnumerable<Service> services = null,
          IEnumerable<Segment> segments = null,
          IEnumerable<Coin.Coin> coins = null,
          IEnumerable<PromotionView> views = null)
    {
        Status = PromotionStatus.ToLaunch;
        StartDate = startDate;
        EndDate = endDate;
        Title = title;
        Description = description;
        //Correlationid = correlationId;
        CreateDate = DateTimeOffset.UtcNow;
        //Services = services?.ToList() ?? [];
        Segments = segments?.ToList() ?? [];
        Coins = coins?.ToList() ?? [];
        Views = views?.ToList() ?? [];
    }

    public int Id { get; set; }
    public decimal? TotalCost { get; private set; }
    public PromotionStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }
    public bool IsDeleted { get; private set; }
    //public Guid Correlationid { get; private set; }
    public string? FromTemplateId { get; set; }
    public int? CreatedByUserId { get; set; }
    public ICollection<Service> Services { get; private set; } = new List<Service>();
    public ICollection<Segment> Segments { get; private set; }
    public ICollection<Coin.Coin> Coins { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; }
    public ICollection<PromotionView> Views { get; private set; }
}
