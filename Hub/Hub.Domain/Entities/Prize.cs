#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Prize : BaseEntity<int>
{
    public Prize()
    {
        
    }

    public Prize(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    public int RewardId { get; private set; }
    public Reward Reward { get; private set; }

    public void ChangeDetails(int value, int probability, int prizeTypeId)
    {
        Value = value;
    }
}