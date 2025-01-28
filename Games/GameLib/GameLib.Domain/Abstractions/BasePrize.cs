#nullable disable

using CheckmateValidations;
using GameLib.Domain.Checkers;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Abstractions;

[CheckMate<BasePrizeChecker>]
public abstract class BasePrize : BaseEntity<int>
{
    private int? _globalLimit;
    private int? _globalSetLimit;

    public string Name { get; set; }
    public int Value { get; set; }
    public int Probability { get; set; }
    public string CoinId { get; set; }

    public int? PerPlayerSetLimit { get; set; }
    public int? GlobalLimit
    {
        get => _globalLimit;
        set
        {
            _globalLimit = value;
            RemainingGlobalLimit = _globalLimit;
        }
    }

    [IgnoreIncludeAll]
    [JsonIgnore]
    public int? RemainingGlobalLimit { get; set; }

    public int? SetSize { get; set; }
    public int? GlobalSetLimit
    {
        get => _globalSetLimit;
        set
        {
            _globalSetLimit = value;
            RemainingGlobalSetLimit = _globalSetLimit;
        }
    }

    [IgnoreIncludeAll]
    [JsonIgnore]
    public int? RemainingGlobalSetLimit { get; set; }

    [IgnoreIncludeAll]
    public int PrizeGroupId { get; set; }

    //public PrizeType PrizeType { get; set; }
    //public int PrizeTypeId { get; set; }
    
    public void DecrementRemainingGlobalLimit()
    {
        if (RemainingGlobalLimit != null && RemainingGlobalLimit > 0)
        {
            RemainingGlobalLimit--;
        }
    }
}

public abstract class BasePrize<TPrizeGroup> : BasePrize
    where TPrizeGroup : BasePrizeGroup
{
    [IgnoreIncludeAll]
    [JsonIgnore] 
    public TPrizeGroup PrizeGroup { get; set; }
}