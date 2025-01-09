#nullable disable

using GameLib.Domain.Abstractions;
using Newtonsoft.Json;

namespace PenaltyKicks.Domain.Entities;

public class PenaltySeries : BasePrizeGroup<PenaltyPrize>
{
    public PenaltySeries()
    {
        
    }

    public PenaltySeries(IEnumerable<PenaltyPrize> prizes = null)
    {
        Prizes = prizes.ToList() ?? [];
    }

    [JsonIgnore]
    public PenaltyConfiguration Configuration { get; set; }
}