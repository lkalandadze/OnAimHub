using AggregationService.Domain.Enum;

namespace AggregationService.Application.Models.AggregationConfigurations;

public class CreateAggregationConfigurationModel
{
    public ConfigurationType ConfigurationType { get; set; }
    public string SpendableFund { get; set; }
    public int FundsSpent { get; set; }
    public string EarnableFund { get; set; }
    public int FundsEarned { get; set; }
    public bool IsRepeatable { get; set; }
    public int ContextId { get; set; }
    public string ContextType { get; set; }
}
