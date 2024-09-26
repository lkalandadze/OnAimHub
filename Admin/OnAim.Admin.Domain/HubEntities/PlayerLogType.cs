#nullable disable

namespace OnAim.Admin.Domain.HubEntities;

public class PlayerLogType : DbEnum<int>
{
    public static PlayerLogType Auth => FromId(1);

    private static PlayerLogType FromId(int id)
    {
        return new PlayerLogType { Id = id };
    }
}