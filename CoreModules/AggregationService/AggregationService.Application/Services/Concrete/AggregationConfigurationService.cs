using AggregationService.Application.Models.Request;
using AggregationService.Application.Models.Response.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

    public async Task<List<string>> ProcessPlayRequestAsync(int playerId, string coinIn, decimal amount, int promotionId)
    {
        // Get all aggregation configurations by PromotionId
        var aggregations = await _aggregationConfigurationRepository.Query()
            .Where(a => a.PromotionId == promotionId.ToString())
            .Include(a => a.PointEvaluationRules)
            .ToListAsync();

        if (!aggregations.Any())
        {
            throw new InvalidOperationException($"No aggregation configurations found for PromotionId {promotionId}");
        }

        var eventPayloads = new List<string>();

        foreach (var aggregation in aggregations)
        {
            // Calculate the points based on PointEvaluationRules
            decimal calculatedAmount = aggregation.EvaluationType switch
            {
                EvaluationType.SingleRule => aggregation.PointEvaluationRules.FirstOrDefault()?.Point ?? 0,
                EvaluationType.Steps => aggregation.PointEvaluationRules
                    .Where(rule => amount >= rule.Step)
                    .OrderByDescending(rule => rule.Step)
                    .FirstOrDefault()?.Point ?? 0,
                _ => throw new InvalidOperationException("Unsupported evaluation type")
            };

            // Create the event payload
            var eventPayload = new
            {
                Key = aggregation.Key,
                CalculatedAmount = calculatedAmount,
                PlayerId = playerId,
                CoinIn = coinIn,
                PromotionId = promotionId
            };

            var message = JsonSerializer.Serialize(eventPayload);
            eventPayloads.Add(message);
        }

        return eventPayloads;
    }
}