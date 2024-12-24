using AggregationService.Domain.Enum;
using Shared.Domain.Entities;

namespace AggregationService.Domain.Entities;

public class AggregationConfiguration : BaseEntity<int>
{
    public AggregationConfiguration(ConfigurationType configurationType, string spendableFund, int fundsSpent, string earnableFund, int fundsEarned, bool isRepeatable)
    {
        ConfigurationType = configurationType;
        SpendableFund = spendableFund;
        FundsSpent = fundsSpent;
        EarnableFund = earnableFund;
        FundsEarned = fundsEarned;
        IsRepeatable = isRepeatable;
    }

    public ConfigurationType ConfigurationType { get; set; }

    //Coins to spend and how much
    public string SpendableFund { get; set; }
    public int FundsSpent { get; set; }

    //Ciubs to earn and how much
    public string EarnableFund { get; set; }
    public int FundsEarned { get; set; }

    public bool IsRepeatable { get; set; }
    public int AggregationId { get; set; }
    public Aggregation Aggregation { get; set; }
}
