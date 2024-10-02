#nullable disable

using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {
        
    }

    public Segment(string id, int? configurationId = null)
    {
        Id = id.ToLower();
        ConfigurationId = configurationId;
    }
    
    public bool IsDeleted { get; private set; }

    public int? ConfigurationId { get; private set; }
    [JsonIgnore]
    public Configuration Configuration { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void SetConfiguration(int configurationId)
    {
        ConfigurationId = configurationId;
    }
}