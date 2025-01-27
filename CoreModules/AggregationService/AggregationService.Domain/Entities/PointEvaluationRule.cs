using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace AggregationService.Domain.Entities;

[NotMapped]
public class PointEvaluationRule
{
    public PointEvaluationRule(int step, int point)
    {
        Id = Guid.NewGuid().ToString();
        Step = step;
        Point = point;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    public int Step { get; set; }
    public int Point { get; set; }

    public void Update(int step, int point)
    {
        Step = step;
        Point = point;
    }
}