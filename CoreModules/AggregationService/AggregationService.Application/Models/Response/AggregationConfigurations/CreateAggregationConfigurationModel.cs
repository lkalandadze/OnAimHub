using AggregationService.Application.Models.Response.Filters;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AggregationService.Application.Models.Response.AggregationConfigurations;

public class CreateAggregationConfigurationModel
{
    public string EventProducer { get; set; }
    public string AggregationSubscriber { get; set; }
    public List<FilterModel> Filters { get; set; }
    public AggregationType AggregationType { get; set; }
    public EvaluationType EvaluationType { get; set; }
    public IEnumerable<PointEvaluationRule> PointEvaluationRules { get; set; } = new List<PointEvaluationRule>();
    public string SelectionField { get; set; }
    public DateTime Expiration { get; set; }
    public string PromotionId { get; set; }
    public string Key { get; set; }
}