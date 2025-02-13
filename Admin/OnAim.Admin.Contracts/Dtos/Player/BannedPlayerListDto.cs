﻿namespace OnAim.Admin.Contracts.Dtos.Player;

public class BannedPlayerListDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public DateTimeOffset DateBanned { get; set; }
    public DateTimeOffset? ExpireDate { get; set; }
    public bool IsPermanent { get; set; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset? RevokeDate { get; set; }
    public string Description { get; set; }
}
