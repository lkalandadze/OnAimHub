#nullable disable

using GameLib.Domain.Entities;

namespace TestWheel.Domain.Entities;

public class TestWheelConfiguration : GameConfiguration<TestWheelConfiguration>
{
    public TestWheelConfiguration()
    {
        
    }

    public TestWheelConfiguration(string name, int value, int promotionId, string description = null, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null, IEnumerable<Round> rounds = null) 
        : base(name, value, promotionId, description, correlationId, templateId, prices)
    {
        Rounds = rounds.ToList() ?? [];
    }

    public ICollection<Round> Rounds { get; set; }
}