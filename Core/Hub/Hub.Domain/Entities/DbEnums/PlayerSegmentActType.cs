using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class PlayerSegmentActType : DbEnum<int, PlayerSegmentActType>
{
    public static PlayerSegmentActType Assign => FromId(1, nameof(Assign));
    public static PlayerSegmentActType Unassign => FromId(2, nameof(Unassign));
    public static PlayerSegmentActType Block => FromId(3, nameof(Block));
    public static PlayerSegmentActType Unblock => FromId(4, nameof(Unblock));
}