using AggregationService.Application.Models.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;
using StackExchange.Redis;

namespace AggregationService.Application.Services.Concrete;

public class AggregationConfigurationService : IAggregationConfigurationService
{
    private readonly IAggregationConfigurationRepository _aggregationConfigurationRepository;
    private readonly IDatabase _db;
    private readonly IMessageBus _messageBus;
    private readonly IConfigurationStore _configurationStore;
    private readonly ILogEntryRepository _logRepository;
    public AggregationConfigurationService(
                                        IAggregationConfigurationRepository aggregationConfigurationRepository,
                                        IConnectionMultiplexer redisConnection,
                                        IMessageBus messageBus,
                                        IConfigurationStore configurationStore,
                                        ILogEntryRepository logRepository)
    {
        _aggregationConfigurationRepository = aggregationConfigurationRepository;
        _db = redisConnection.GetDatabase();
        _messageBus = messageBus;
        _configurationStore = configurationStore;
        _logRepository = logRepository;
    }

    public async Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model)
    {
        var aggregation = new AggregationConfiguration(
            model.EventProducer,
            model.AggregationSubscriber,
            model.Filters.Select(f => new Filter(f.Property, f.Operator, f.Value)).ToList(),
            model.AggregationType,
            model.EvaluationType,
            model.PointEvaluationRules.Select(p => new PointEvaluationRule(p.Step, p.Point)).ToList(),
            model.SelectionField,
            model.Expiration,
            model.PromotionId,
            model.Key
        );

        await _aggregationConfigurationRepository.AddConfigurationsAsync(new List<AggregationConfiguration> { aggregation });
    }

    public async Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model)
    {
        var filter = Builders<AggregationConfiguration>.Filter.Eq(x => x.Id, model.Id);
        var aggregation = await _aggregationConfigurationRepository.GetCollection().Find(filter).FirstOrDefaultAsync();

        if (aggregation == default)
            throw new KeyNotFoundException($"Aggregation configuration with ID {model.Id} not found.");

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

        await _aggregationConfigurationRepository.UpdateAsync(aggregation, filter);
    }

    public async Task TriggerRequestAsync(AggregationTriggerEvent @event, AggregationConfiguration config)
    {
        var eventValue = @event.ExtractValue(config);
        if (eventValue > 0)
        {
            //make atomic with lock
            var currentTotal = _db.StringIncrement(config.GenerateKeyForValue(@event), (double)eventValue);

            var currentPoints = config.CalculatePoints(currentTotal);

            if (currentPoints > 0)
            {

                var dbPreviousRecord = _db.StringGet(config.GenerateKeyForPoints(@event));
                double.TryParse(dbPreviousRecord!, out double previousPoints);

                if (currentPoints > previousPoints)
                {
                    _db.StringSet(config.GenerateKeyForPoints(@event), currentPoints);

                    var pointsAdded = currentPoints - previousPoints;

                    var eventName = $"{config.AggregationSubscriber}Queue";


                    //_ = _messageBus.PublishWithRouting(new AggregatedEvent
                    //{
                    //    PlayerId = @event.CustomerId,
                    //    Timestamp = DateTime.Now,
                    //    AddedPoints = pointsAdded,
                    //    PromotionId = config.PromotionId,
                    //    ConfigKey = config.Key,
                    //}, eventName);

                    var aggregatedEvent = new AggregatedEvent
                    {
                        PlayerId = @event.CustomerId,
                        Timestamp = DateTime.Now,
                        AddedPoints = pointsAdded,
                        PromotionId = config.PromotionId,
                        ConfigKey = config.Key,
                    };

                    _ = _messageBus.PublishWithRouting(aggregatedEvent, eventName);

                    await _logRepository.LogEventAsync(
                            subscriber: config.AggregationSubscriber,
                            producer: config.Key,
                            eventDetails: System.Text.Json.JsonSerializer.Serialize(aggregatedEvent)
                    );
                }

                Console.WriteLine("TEST TRIGGER");
            }
        }
    }

    public async Task Test(AggregationTriggerEvent test, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received event: {System.Text.Json.JsonSerializer.Serialize(test)}");

        await _configurationStore.ReloadConfigurationsAsync();

        var filteredConfigurations = _configurationStore.GetAllConfigurations().Filter(test);

        if (!filteredConfigurations.Any())
        {
            throw new InvalidOperationException($"No matching configurations found. Request: {System.Text.Json.JsonSerializer.Serialize(test)}");
        }

        var promotionIds = filteredConfigurations
            .Select(config => config.PromotionId)
            .Distinct()
            .ToList();

        Console.WriteLine($"Promotion IDs: {string.Join(", ", promotionIds)}");

        var filter = test.IsExternal
            ? Builders<AggregationConfiguration>.Filter.Empty
            : Builders<AggregationConfiguration>.Filter.In(config => config.PromotionId, promotionIds);

        var configurations = await _aggregationConfigurationRepository
            .GetCollection()
            .Find(filter)
            .ToListAsync(cancellationToken);

        if (!configurations.Any())
        {
            throw new InvalidOperationException($"No configurations found for PromotionIds: {string.Join(", ", promotionIds)}");
        }

        foreach (var config in configurations)
        {
            await TriggerRequestAsync(test, config);
        }

        Console.WriteLine($"Trigger aggregation successfully processed. with {configurations.Count} configurations. from event: \n {System.Text.Json.JsonSerializer.Serialize(test)}");
    }
}