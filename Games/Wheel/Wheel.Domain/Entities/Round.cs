using GameLib.Domain.Entities;
using Shared.Domain.Entities;

namespace Wheel.Domain.Entities;

public class Round : BaseEntity<int>
{
    public Round()
    {
    }

    public string Name { get; set; }
    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }
}