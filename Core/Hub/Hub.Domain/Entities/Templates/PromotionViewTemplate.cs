#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities.Templates;

public class PromotionViewTemplate : BaseEntity<int>
{
    public PromotionViewTemplate()
    {

    }

    public PromotionViewTemplate(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; private set; }
    public string Url { get; private set; }
}