using AggregationService.Domain.Enum;
using Shared.Domain.Entities;

public class Filter : BaseEntity<string>
{
    public Filter(string property, Operator @operator, string value)
    {
        Id = Guid.NewGuid().ToString();
        Property = property;
        Operator = @operator;
        Value = value;
    }

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