﻿namespace Hub.Application.Models.Game;

public class ActiveGameModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Address { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
}
