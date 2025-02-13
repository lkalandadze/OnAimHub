﻿using AggregationService.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace OnAim.Admin.Domain.HubEntities.Coin;

[BsonDiscriminator("InCoin")]
public class InCoin : Coin
{
    public IEnumerable<AggregationConfiguration> AggregationConfiguration { get; set; }

    public InCoin()
    {

    }

    public InCoin(string id, string name, string description, string imageUrl, int promotionId, IEnumerable<AggregationConfiguration> aggregationConfigurations, string? templateId = null)
        : base(id, name, description, imageUrl, Domain.HubEntities.Enum.CoinType.In, promotionId, templateId)
    {
        AggregationConfiguration = aggregationConfigurations.ToList() ?? [];
    }
}
