using AggregationService.Application.Models.Response.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;
using StackExchange.Redis;

namespace AggregationService.Application.Services.Concrete;

public class AggregationConfigurationService : IAggregationConfigurationService
{
    private readonly IAggregationConfigurationRepository _aggregationConfigurationRepository;
    private readonly IDatabase _db;
    private readonly IMessageBus _messageBus;
    public AggregationConfigurationService(
                                        IAggregationConfigurationRepository aggregationConfigurationRepository,
                                        IConnectionMultiplexer redisConnection,
                                        IMessageBus messageBus)
    {
        _aggregationConfigurationRepository = aggregationConfigurationRepository;
        _db = redisConnection.GetDatabase();
        _messageBus = messageBus;
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

    public async Task TriggerRequestAsync(TriggerAggregationEvent @event, AggregationConfiguration config)
    {
        var eventValue = @event.ExtractValue(config);
        if (eventValue > 0)
        {
            //make atomic with lock
            var currentTotal = _db.StringIncrement(config.GenerateKeyForValue(@event), (double)eventValue);

            var currentPoints = config.CalculatePoints(currentTotal);

            if (currentPoints > 0)
            {
                var previousPoints = double.Parse(_db.StringGet(config.GenerateKeyForPoints(@event)));


                if (previousPoints > currentPoints)
                {
                    _db.StringSet(config.GenerateKeyForPoints(@event), currentPoints);

                    var pointsAdded = currentPoints - previousPoints;

                    _ = _messageBus.PublishWithRouting(new AggregatedEvent
                    {
                        PlayerId = @event.CustomerId,
                        Timestamp = DateTime.Now,
                        AddedPoints = pointsAdded,
                        PromotionId = config.PromotionId,
                        ConfigKey = config.Key,
                    }, config.AggregationSubscriber);
                }
            }
        }
    }
}