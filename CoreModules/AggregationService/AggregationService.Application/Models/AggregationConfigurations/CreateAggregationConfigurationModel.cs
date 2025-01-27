using AggregationService.Application.Models.Filters;
using AggregationService.Application.Models.PointEvaluationRules;
using AggregationService.Domain.Enum;

namespace AggregationService.Application.Models.AggregationConfigurations;

public class CreateAggregationConfigurationModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    //"external" when sent from casion
    //"Hub" when sent from hub
    public string EventProducer { get; set; }
    //"Leaderboard" when LB wants to give scores according to (e.g.) bet
    public string AggregationSubscriber { get; set; }
    //"eventType:bet"
    public List<FilterModel> Filters { get; set; }
    public AggregationType AggregationType { get; set; }
    public EvaluationType EvaluationType { get; set; }
    public IEnumerable<PointEvaluationRuleModel> PointEvaluationRules { get; set; }
    public string SelectionField { get; set; }
    public DateTime Expiration { get; set; }
    public string PromotionId { get; set; }
    public string Key { get; set; }
}