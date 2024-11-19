﻿#nullable disable

using Hub.Domain.Entities.DbEnums;
using MassTransit.Internals.GraphValidation;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerBalance : BaseEntity<int>
{
    public PlayerBalance()
    {
        
    }

    public PlayerBalance(decimal amount, int playerId, string currencyId)
    {
        Amount = amount;
        PlayerId = playerId;
        CurrencyId = currencyId;
        //PromotionId = promotionId;
    }

    public decimal Amount { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public int PromotionId { get; private set; }
    public Promotion Promotion { get; private set; }

    public void SetAmount(decimal amount)
    {
        Amount = amount;
    }
}