#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerLogType : DbEnum<int>
{
    public static PlayerLogType Auth => FromId(1);

    private static PlayerLogType FromId(int id)
    {
        return new PlayerLogType { Id = id };
    }
}