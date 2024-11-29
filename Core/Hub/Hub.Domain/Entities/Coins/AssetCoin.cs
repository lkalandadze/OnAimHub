﻿using Hub.Domain.Enum;

namespace Hub.Domain.Entities.Coins;

public class AssetCoin : Coin
{
    public AssetCoin()
    {
        
    }

    public AssetCoin(string id, string name, string description, string imageUrl, int promotionId)
        : base(id, name, description, imageUrl, CoinType.Asset, promotionId)
    {
    }
}