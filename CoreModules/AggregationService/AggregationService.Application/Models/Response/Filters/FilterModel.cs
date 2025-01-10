using AggregationService.Domain.Enum;

namespace AggregationService.Application.Models.Response.Filters;

public class FilterModel
{
    public string Property { get; set; }
    public Operator Operator { get; set; }
    public string Value { get; set; }
}