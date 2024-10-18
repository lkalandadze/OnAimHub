using GameLib.Domain.Abstractions;
using System.Text.Json.Serialization;

namespace Wheel.Domain.Entities;

public class Round : BasePrizeGroup<WheelPrize>
{
    public Round()
    {
    }

    public string Name { get; set; }
    [JsonIgnore]
    public WheelConfiguration Configuration { get; set; }
    public ICollection<WheelPrize> Prizes { get; set; }
}