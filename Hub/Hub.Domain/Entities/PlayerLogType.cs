#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerLogType : DbEnum<int, PlayerLogType>
{
    public static PlayerLogType Auth => FromId(1);
}