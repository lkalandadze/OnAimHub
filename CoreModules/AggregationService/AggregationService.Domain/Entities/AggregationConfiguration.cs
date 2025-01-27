using AggregationService.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Domain.Entities;

namespace AggregationService.Domain.Entities;

// Only available when used with 1 CoinIn
public class AggregationConfiguration
{
    public AggregationConfiguration()
    {
        
    }
    public AggregationConfiguration(
        string eventProducer,
        string aggregationSubscriber,
        List<Filter> filters,
        AggregationType aggregationType,
        EvaluationType evaluationType,
        IEnumerable<PointEvaluationRule> pointEvaluationRules,
        string selectionField,
        DateTime expiration,
        string promotionId,
        string key)
    {
        Id = Guid.NewGuid().ToString();
        EventProducer = eventProducer;
        AggregationSubscriber = aggregationSubscriber;
        Filters = filters;
        AggregationType = aggregationType;
        EvaluationType = evaluationType;
        PointEvaluationRules = pointEvaluationRules;
        SelectionField = selectionField;
        Expiration = expiration;
        PromotionId = promotionId;
        Key = key;
    }
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public string EventProducer { get; set; } // Example: Hub
    public string AggregationSubscriber { get; set; } // Example: Leaderboard
    public List<Filter> Filters { get; set; } = new();
    public AggregationType AggregationType { get; set; }
    public EvaluationType EvaluationType { get; set; }
    public IEnumerable<PointEvaluationRule> PointEvaluationRules { get; set; } = new List<PointEvaluationRule>();
    public string SelectionField { get; set; } // Example: "Bet"
    public DateTime Expiration { get; set; }
    public string PromotionId { get; set; }
    public string Key { get; set; } // Example: LeaderboardId

    public void Update(
        string eventProducer,
        string aggregationSubscriber,
        List<Filter> filters,
        AggregationType aggregationType,
        EvaluationType evaluationType,
        IEnumerable<PointEvaluationRule> pointEvaluationRules,
        string selectionField,
        DateTime expiration,
        string promotionId,
        string key)
    {
        EventProducer = eventProducer;
        AggregationSubscriber = aggregationSubscriber;
        Filters = filters;
        AggregationType = aggregationType;
        EvaluationType = evaluationType;
        PointEvaluationRules = pointEvaluationRules;
        SelectionField = selectionField;
        Expiration = expiration;
        PromotionId = promotionId;
        Key = key;
    }
}