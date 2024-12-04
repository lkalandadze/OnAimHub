﻿#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateAssetCoinModel : CreateCoinModel
{
    public int? TemplateId { get; set; }
    // should be add configuration in future

    public CreateAssetCoinModel()
    {
        CoinType = CoinType.Asset;
    }
}