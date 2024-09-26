#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {
        
    }

    public Segment(string id, int configurationId)
    {
        Id = id.ToLower();
        ConfigurationId = configurationId;
    }
    
    public bool IsDeleted { get; private set; }

    public int ConfigurationId { get; private set; }
    public Configuration Configuration { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }
}