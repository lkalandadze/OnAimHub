﻿#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PromotionCoin : BaseEntity<string>
{
    public PromotionCoin()
    {
        
    }

    public PromotionCoin(string id, string name, string description, string imageUrl, CoinType coinType, int promotionId, int? fromTemplateId = null)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
        PromotionId = promotionId;
        FromTemplateId = fromTemplateId;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public int? FromTemplateId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public ICollection<WithdrawOption> WithdrawOptions { get; set; }

    public void Delete()
    { 
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }
}