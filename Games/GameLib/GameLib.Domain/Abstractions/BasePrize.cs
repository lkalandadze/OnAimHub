#nullable disable
using CheckmateValidations;
using GameLib.Domain.Checkers;
using GameLib.Domain.Entities;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Abstractions;

[CheckMate<BasePrizeChecker>]
public abstract class BasePrize : BaseEntity<int>
{
    public int Value { get; set; }
    public int Probability { get; set; }
    public string CoinId { get; set; }

    [IgnoreIncludeAll]
    public int PrizeGroupId { get; set; }

    //public PrizeType PrizeType { get; set; }
    //public int PrizeTypeId { get; set; }
}

public abstract class BasePrize<TPrizeGroup> : BasePrize
    where TPrizeGroup : BasePrizeGroup
{
    [IgnoreIncludeAll]
    [JsonIgnore] 
    public TPrizeGroup PrizeGroup { get; set; }
}