using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class PlayerSegmentActType : DbEnum<int, PlayerSegmentActType>
{
    public static PlayerSegmentActType Assign => FromId(1);
    public static PlayerSegmentActType Unassign => FromId(2);
    public static PlayerSegmentActType Block => FromId(3);
    public static PlayerSegmentActType Unblock => FromId(4);
}