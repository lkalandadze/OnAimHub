#nullable disable

using GameLib.Domain.Entities;

namespace Wheel.Domain.Entities;

public class WheelConfiguration : GameConfiguration<WheelConfiguration>
{
    public WheelConfiguration()
    {
        
    }

    public WheelConfiguration(string name, int value, IEnumerable<Price>? prices = null, IEnumerable<Segment>? segments = null, IEnumerable<Round> rounds = null) : base(name, value, prices, segments)
    {
        Rounds = rounds.ToList() ?? [];
    }

    public ICollection<Round> Rounds { get; set; }
}