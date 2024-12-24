namespace SagaOrchestrationStateMachine.Models;

public class PromotionDto
{
    public int Id { get; set; }
    public decimal? TotalCost { get; set; }
    public string Status { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public Guid CorrelationId { get; set; }
    public string FromTemplateId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    // Related data
    public List<PromotionServiceDto> Services { get; set; }
    public List<SegmentDto> Segments { get; set; }
    public List<CoinDto> Coins { get; set; }
    public List<PromotionViewDto> Views { get; set; }
}
public class PromotionServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class SegmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CoinDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}

public class PromotionViewDto
{
    public int Id { get; set; }
    public string ViewName { get; set; }
}