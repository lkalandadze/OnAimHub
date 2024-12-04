﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities.Coin;

public abstract class Coin
{
    public Coin(){}

    public Coin(string id,
         string name,
         string description,
         string imageUrl,
         CoinType coinType)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }
}