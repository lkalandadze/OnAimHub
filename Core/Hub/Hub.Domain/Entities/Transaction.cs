﻿#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.DbEnums;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class Transaction : BaseEntity<int>
{
    public Transaction()
    {
        
    }

    public Transaction(decimal amount, int? keyId, string? sourceServiceName, int playerId, AccountType fromAccount, AccountType toAccount, string coinId, TransactionStatus status, TransactionType type, int? promotionId)
    {
        Amount = amount;
        KeyId = keyId;
        SourceServiceName = sourceServiceName;
        PlayerId = playerId;
        FromAccountId = fromAccount.Id;
        ToAccountId = toAccount.Id;
        CoinId = coinId;
        StatusId = status.Id;
        TypeId = type.Id;
        PromotionId = promotionId;
        DateCreated = DateTime.UtcNow;
    }

    public decimal Amount { get; private set; }

    public int? KeyId { get; private set; }
    public string SourceServiceName {get; private set; }
    //public Service Service { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public int FromAccountId { get; private set; }
    public AccountType FromAccount { get; private set; }

    public int ToAccountId { get; private set; }
    public AccountType ToAccount { get; private set; }

    public string CoinId { get; private set; }
    public Coin Coin { get; private set; }

    public int StatusId { get; private set; }
    public TransactionStatus Status { get; private set; }

    public int TypeId { get; private set; }
    public TransactionType Type { get; private set; }

    public int? PromotionId { get; private set; }
    public Promotion Promotion { get; private set; }

    public DateTimeOffset DateCreated { get; private set; }

    public void SetStatus(TransactionStatus status)
    {
        StatusId = status.Id;
    }
}