#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Newtonsoft.Json;
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

    [JsonIgnore]
    public PenaltyConfiguration Configuration { get; set; }
}