#nullable disable

using Hub;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class PlayerLogType : DbEnum<int, PlayerLogType>
{
    public static PlayerLogType Auth => FromId(1);
}