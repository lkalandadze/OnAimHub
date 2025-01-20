#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Newtonsoft.Json;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using PenaltyKicks.Domain.Checkers;

namespace PenaltyKicks.Domain.Entities;

[CheckMate<PenaltyPrizeGroupChecker>]
public class PenaltyPrizeGroup : BasePrizeGroup<PenaltyPrize>
{
    public PenaltyPrizeGroup()
    {
        
    }

    public PenaltyPrizeGroup(IEnumerable<PenaltyPrize> prizes = null)
    {
        Prizes = prizes.ToList() ?? [];
    }

    [IgnoreIncludeAll]
    [JsonIgnore]
    public PenaltyConfiguration Configuration { get; set; }
}