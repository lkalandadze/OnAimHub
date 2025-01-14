﻿using Newtonsoft.Json;
using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Aggregation;

public class TriggerAggregationEvent : IIntegrationEvent
{
    public Dictionary<string, string> Data { get; set; }
    public string CustomerId => Data["customerId"];
    public string EventType => Data["eventType"];
    public string Producer { get; private set; } = "hub";
    public bool IsExternal => !string.IsNullOrEmpty(Producer) && Producer.ToLower() == "external";
    public Guid CorrelationId {  get; private set; }

    public TriggerAggregationEvent() { }

    public TriggerAggregationEvent(string data, string? producer = null)
    {
        Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(data)!;
        Producer = producer ?? Data["producer"];
        CorrelationId = Guid.NewGuid();
    }
}