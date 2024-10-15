#nullable disable

using CheckmateValidations;
using GameLib.Domain.Checkers;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

[CheckMate<SegmentChecker>]
public class Segment : BaseEntity<string>
{
    public bool IsDeleted { get; private set; }

    public Segment(string id)
    {
        Id = id.ToLower();
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void SetConfiguration()
    {
    }
}

public class Segment<T> : Segment where T : Segment<T>
{
    public Segment(string id, int? configurationId = null) : base(id)
    {
        // Initialization logic for the generic segment, if any
    }

    [JsonIgnore]
    public T Configuration { get; private set; }
}