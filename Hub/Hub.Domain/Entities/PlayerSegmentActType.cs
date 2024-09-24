using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerSegmentActType : DbEnum<int>
{
    public static PlayerSegmentActType Assign => FromId(1);
    public static PlayerSegmentActType Unassign => FromId(2);
    public static PlayerSegmentActType Block => FromId(3);
    public static PlayerSegmentActType Unblock => FromId(4);

    private static PlayerSegmentActType FromId(int id)
    {
        return new PlayerSegmentActType { Id = id };
    }
}