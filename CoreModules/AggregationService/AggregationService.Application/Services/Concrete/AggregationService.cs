using AggregationService.Application.Models.Aggregations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AggregationService.Application.Services.Concrete;

public class AggregationService : IAggregationService
{
    private readonly IAggregationRepository _aggregationRepository;
    public AggregationService(IAggregationRepository aggregationRepository)
    {
        _aggregationRepository = aggregationRepository;
    }

    public async Task AddAggregationWithConfigurationsAsync(CreateAggregationModel request)
    {
        var aggregation = new Aggregation(
            request.Title,
            request.Description,
            request.PromotionId,
            request.StartDate,
            request.EndDate
        );

        foreach (var config in request.AggregationConfigurations)
        {
            aggregation.AddAggregationConfigurations(
                config.ConfigurationType,
                config.SpendableFund,
                config.FundsSpent,
                config.EarnableFund,
                config.FundsEarned,
                config.IsRepeatable,
                config.ContextId,
                config.ContextType
            );
        }

        await _aggregationRepository.InsertAsync(aggregation);
        await _aggregationRepository.SaveChangesAsync();
    }

    public async Task UpdateAggregationAsync(UpdateAggregationModel request)
    {
        var aggregation = await _aggregationRepository.Query().Include(x => x.AggregationConfigurations).FirstOrDefaultAsync(x => x.Id == request.Id);

        if (aggregation == default)
            throw new KeyNotFoundException($"Aggregation with ID {request.Id} not found.");

        aggregation.Update(
            request.Title,
            request.Description
        );

        var updatedConfigurations = request.AggregationConfigurations.Select(config => new AggregationConfiguration(
            config.ConfigurationType,
            config.SpendableFund,
            config.FundsSpent,
            config.EarnableFund,
            config.FundsEarned,
            config.IsRepeatable,
            default, // Pass default for contextId to preserve existing value
            default  // Pass default for contextType to preserve existing value
        )
        {
            Id = config.Id // Set the ID explicitly to update the existing record
        }).ToList();

        aggregation.UpdateConfigurations(updatedConfigurations);

        await _aggregationRepository.SaveChangesAsync();
    }
}