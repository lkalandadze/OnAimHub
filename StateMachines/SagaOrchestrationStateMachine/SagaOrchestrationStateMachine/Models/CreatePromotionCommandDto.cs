using Hub.Application.Models.Coin;

namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionCommandDto
{
    public string Title { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Description { get; set; }
    public Guid CorrelationId { get; set; }
    public string? TemplateId { get; set; }
    public int? CreatedByUserId { get; set; }
    public IEnumerable<string> SegmentIds { get; set; }
    public IEnumerable<CreateCoinModel> Coins { get; set; }
    public IEnumerable<int> ServiceIds { get; set; }
}
