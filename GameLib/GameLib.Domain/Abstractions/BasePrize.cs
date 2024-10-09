using GameLib.Domain.Entities;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Abstractions;

public abstract class BasePrize : BaseEntity<int>
{
    public int Value { get; set; }
    public int Probability { get; set; }
    public int PrizeTypeId { get; set; }
    public PrizeType PrizeType { get; set; }
    public int PrizeGroupId { get; set; }
}

public abstract class BasePrize<TPrizeGroup> : BasePrize
    where TPrizeGroup : BasePrizeGroup
{
    public TPrizeGroup PrizeGroup { get; set; }
}