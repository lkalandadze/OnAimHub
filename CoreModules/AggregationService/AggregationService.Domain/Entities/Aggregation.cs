using AggregationService.Domain.Enum;
using Shared.Domain.Entities;
using System.Linq;

namespace AggregationService.Domain.Entities;

public class Aggregation : BaseEntity<int>
{
    public Aggregation(string title, string description, int promotionId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        Title = title;
        Description = description;
        PromotionId = promotionId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public int PromotionId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public ICollection<AggregationConfiguration> AggregationConfigurations { get; set; } = new List<AggregationConfiguration>();


    public void AddAggregationConfigurations(ConfigurationType configurationType, string spendableFund, int fundsSpent, string earnableFund, int fundsEarned, bool isRepeatable, int contextId, string contextType)
    {
        var prize = new AggregationConfiguration(configurationType, spendableFund, fundsSpent, earnableFund, fundsEarned, isRepeatable, contextId, contextType);
        AggregationConfigurations.Add(prize);
    }

    public void Update(string title, string description)
    {
        Title = title; 
        Description = description;
    }

    public void UpdateConfigurations(IEnumerable<AggregationConfiguration> updatedConfigurations)
    {
        foreach (var updatedConfig in updatedConfigurations)
        {
            var existingConfig = AggregationConfigurations.FirstOrDefault(c => c.Id == updatedConfig.Id);

            if (existingConfig != null)
            {
                existingConfig.Update(
                    updatedConfig.ConfigurationType,
                    updatedConfig.SpendableFund,
                    updatedConfig.FundsSpent,
                    updatedConfig.EarnableFund,
                    updatedConfig.FundsEarned,
                    updatedConfig.IsRepeatable
                );
            }
            else
            {
                throw new Exception("Configuration not found");
            }
        }
    }
}
