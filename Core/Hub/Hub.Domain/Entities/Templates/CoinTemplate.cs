#nullable disable

using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities.Templates;

public class CoinTemplate : BaseEntity<int>
{
    public CoinTemplate()
    {

    }

    public CoinTemplate(string name, string description, string imageUrl, CoinType coinType)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
    }

    public string Name { get; private set; }
    public string Description { get; set; }
    public string ImageUrl { get; private set; }
    public CoinType CoinType { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public void Update(string name, string description, string imageUrl, CoinType coinType)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}