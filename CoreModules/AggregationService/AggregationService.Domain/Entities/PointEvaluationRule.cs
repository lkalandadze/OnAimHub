using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AggregationService.Domain.Entities;

[NotMapped]
public class PointEvaluationRule
{
    public PointEvaluationRule(int step, int point)
    {
        Step = step;
        Point = point;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public int Step { get; set; }
    public int Point { get; set; }

    public void Update(int step, int point)
    {
        Step = step;
        Point = point;
    }
}