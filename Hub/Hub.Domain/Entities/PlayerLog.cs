#nullable disable

using Hub.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PlayerLog : BaseEntity<int>
{
    public PlayerLog()
    {
        Timestamp = DateTimeOffset.UtcNow;
        Log = string.Empty;
    }

    public PlayerLog(string log, int playerId, PlayerLogType playerLogType)
    {
        Log = log;
        Timestamp = DateTimeOffset.UtcNow;
        PlayerId = playerId;
        PlayerLogTypeId = playerLogType.Id;
    }

    public string Log { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public int PlayerLogTypeId { get; set; }
    public PlayerLogType PlayerLogType { get; set; }
}