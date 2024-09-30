#nullable disable

namespace OnAim.Admin.Domain.HubEntities.DbEnums;

public class PlayerLogType : DbEnum<int, PlayerLogType>
{
    public static PlayerLogType Auth => FromId(1);
}