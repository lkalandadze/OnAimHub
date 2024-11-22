#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities.Templates;

public class PromotionViewTemplate : BaseEntity<int>
{
    public PromotionViewTemplate()
    {

    }

    public PromotionViewTemplate(string name, string url, IEnumerable<PromotionView> promotionViews = null)
    {
        Name = name;
        Url = url;
        PromotionViews = promotionViews?.ToList() ?? [];
    }

    public string Name { get; private set; }
    public string Url { get; private set; }

    public ICollection<PromotionView> PromotionViews { get; private set; }
}