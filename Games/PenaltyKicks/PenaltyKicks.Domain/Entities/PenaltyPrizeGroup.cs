#nullable disable

using GameLib.Domain.Abstractions;
using Newtonsoft.Json;

namespace PenaltyKicks.Domain.Entities;

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