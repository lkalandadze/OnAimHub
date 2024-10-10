using GameLib.Domain.Entities;

namespace Wheel.Domain.Entities;

public class WheelConfiguration : GameConfiguration<WheelConfiguration>
{
    public WheelConfiguration()
    {
        
    }

    public WheelConfiguration(string name, int value, IEnumerable<Price>? prices = null, IEnumerable<Segment>? segments = null) : base(name, value, prices, segments)
    {

    }

    public ICollection<Round> Rounds { get; set; }
}