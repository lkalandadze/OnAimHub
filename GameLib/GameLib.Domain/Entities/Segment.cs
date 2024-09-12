using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Segment : BaseEntity<string>
{
    public bool IsActive { get; set; }

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }
}