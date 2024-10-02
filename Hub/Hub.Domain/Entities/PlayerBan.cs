using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerBan : BaseEntity<int>
{
    public PlayerBan(int playerId, DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        PlayerId = playerId;
        ExpireDate = expireDate;
        IsPermanent = isPermanent;
        Description = description;
        DateBanned = DateTimeOffset.UtcNow;
    }

    public int PlayerId { get; set; }
    public Player Player { get; set; }
    public DateTimeOffset DateBanned { get; set; }
    public DateTimeOffset? ExpireDate { get; set; }
    public bool IsPermanent { get; set; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset? RevokeDate { get; set; }
    public string Description { get; set; }

    public void Revoke()
    {
        IsRevoked = true;
        RevokeDate = DateTimeOffset.UtcNow;
    }

    public void Update(DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        ExpireDate = expireDate;
        IsPermanent = isPermanent;
        Description = description;
    }
}
