using AggregationService.Application.Models.Filters;
using AggregationService.Application.Models.PointEvaluationRules;
using AggregationService.Domain.Enum;

namespace AggregationService.Application.Models.AggregationConfigurations;

public class CreateAggregationConfigurationModel
{
    public string EventProducer { get; set; }
    public string AggregationSubscriber { get; set; }
    public List<FilterModel> Filters { get; set; }
    public AggregationType AggregationType { get; set; }
    public EvaluationType EvaluationType { get; set; }
    public IEnumerable<PointEvaluationRuleModel> PointEvaluationRules { get; set; }
    public string SelectionField { get; set; }
    public DateTime Expiration { get; set; }
    public string PromotionId { get; set; }
    public string Key { get; set; }
}