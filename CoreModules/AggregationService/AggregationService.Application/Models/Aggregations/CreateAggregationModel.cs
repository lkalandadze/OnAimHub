using AggregationService.Application.Models.AggregationConfigurations;

namespace AggregationService.Application.Models.Aggregations;

public class CreateAggregationModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PromotionId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }


    public List<CreateAggregationConfigurationModel> AggregationConfigurations { get; set; }
}
