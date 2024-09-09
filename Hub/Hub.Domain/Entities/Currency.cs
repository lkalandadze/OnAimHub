#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Currency : BaseEntity<string>
{
    public Currency()
    {

    }

    public Currency(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public void ChangeDetails(string name)
    {
        Name = name;
    }
}