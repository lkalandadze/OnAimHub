﻿#nullable disable

using Hub.Domain.Entities.DbEnums;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerSegmentAct : BaseEntity<int>
{
    public PlayerSegmentAct()
    {
        
    }

    public PlayerSegmentAct(PlayerSegmentActType action, int totalPlayers, string segmentId, int? addedByUserId = null)
    {
        ActionId = action.Id;
        TotalPlayers = totalPlayers;
        ByUserId = addedByUserId;
        SegmentId = segmentId;
    }

    public int TotalPlayers { get; set; }
    public int? ByUserId { get; set; }
    public bool IsBulk { get; private set; }

    public int? ActionId { get; set; }
    public PlayerSegmentActType Action { get; set; }

    public string SegmentId { get; set; }  
    public Segment Segment { get; set; }

    public void SetIsBulk()
    {
        IsBulk = true;
    }
}