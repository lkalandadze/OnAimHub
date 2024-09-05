using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Currency : BaseEntity<string>
{
    public string Name { get; set; }
}