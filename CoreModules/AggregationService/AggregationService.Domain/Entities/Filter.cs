using AggregationService.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Domain.Entities;

public class Filter
{
    public Filter(string property, Operator @operator, string value)
    {
        Property = property;
        Operator = @operator;
        Value = value;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string Property { get; set; }
    public Operator Operator { get; set; }
    public string Value { get; set; }

    public void Update(string property, Operator @operator, string value)
    {
        Property = property;
        Operator = @operator;
        Value = value;
    }
}