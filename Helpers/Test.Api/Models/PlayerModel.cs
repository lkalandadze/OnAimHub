﻿#nullable disable

namespace Test.Api.Models;

public class PlayerModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public List<string> SegmentIds { get; set; }
}