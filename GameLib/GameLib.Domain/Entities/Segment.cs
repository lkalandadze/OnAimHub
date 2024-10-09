#nullable disable
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {
        
    }

    public bool IsDeleted { get; set; }

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
    public Segment()
    {
        
    }
    public Segment(string id, int? configurationId = null) : base(id)
    {
        // Initialization logic for the generic segment, if any
    }

    [JsonIgnore]
    public T Configuration { get; private set; }
}