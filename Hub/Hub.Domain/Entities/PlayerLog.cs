using Shared.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Hub.Domain.Entities;

public class PlayerLog : BaseEntity<int>
{
    public DateTimeOffset Timestamp { get; set; }
    public string Log { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public int PlayerLogTypeId { get; set; }
    public PlayerLogType PlayerLogType { get; set; }
}