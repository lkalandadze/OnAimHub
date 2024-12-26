using AggregationService.Application.Models.AggregationConfigurations;

namespace AggregationService.Application.Models.Aggregations;

public class UpdateAggregationModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }


    public List<UpdateAggregationConfigurationModel> AggregationConfigurations { get; set; }
}
