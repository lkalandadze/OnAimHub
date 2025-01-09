using AggregationService.Application.Models.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AggregationService.Application.Services.Concrete;

public class AggregationConfigurationService : IAggregationConfigurationService
{
    private readonly IAggregationConfigurationRepository _aggregationConfigurationRepository;
    public AggregationConfigurationService(IAggregationConfigurationRepository aggregationConfigurationRepository)
    {
        _aggregationConfigurationRepository = aggregationConfigurationRepository;
    }

    public async Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model)
    {
        var aggregation = new AggregationConfiguration(
            model.EventProducer,
            model.AggregationSubscriber,
            model.Filters.Select(f => new Filter(f.Property, f.Operator, f.Value)).ToList(),
            model.AggregationType,
            model.EvaluationType,
            model.PointEvaluationRules,
            model.SelectionField,
            model.Expiration,
            model.PromotionId,
            model.Key
        );

        await _aggregationConfigurationRepository.InsertAsync(aggregation);
        await _aggregationConfigurationRepository.SaveChangesAsync();
    }

    public async Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model)
    {
        var aggregation = await _aggregationConfigurationRepository.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

        if (aggregation != default)
        {
            aggregation.Update(
                model.EventProducer,
                model.AggregationSubscriber,
                model.Filters,
                model.AggregationType,
                model.EvaluationType,
                model.PointEvaluationRules,
                model.SelectionField,
                model.Expiration,
                model.PromotionId,
                model.Key
            );

            await _aggregationConfigurationRepository.SaveChangesAsync();
        }
    }
}