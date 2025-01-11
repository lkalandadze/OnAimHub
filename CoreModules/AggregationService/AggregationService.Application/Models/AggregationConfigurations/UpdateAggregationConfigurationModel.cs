using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;

namespace AggregationService.Application.Models.AggregationConfigurations;

public class UpdateAggregationConfigurationModel
{
    public string Id { get; set; }
    public string EventProducer { get; set; }
    public string AggregationSubscriber { get; set; }
    public List<Filter> Filters { get; set; } = new();
    public AggregationType AggregationType { get; set; }
    public EvaluationType EvaluationType { get; set; }
    public IEnumerable<PointEvaluationRule> PointEvaluationRules { get; set; } = new List<PointEvaluationRule>();
    public string SelectionField { get; set; }
    public DateTime Expiration { get; set; }
    public string PromotionId { get; set; }
    public string Key { get; set; }
}
