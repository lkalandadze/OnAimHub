#nullable disable

using Hub.Domain.Entities.Templates;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class PromotionView : BaseEntity<int>
{
    public PromotionView()
    {
        
    }

    public PromotionView(string name, string url, int promotionId, int? templateId = null)
    {
        Name = name;
        Url = url;
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public string Name { get; private set; }
    public string Url { get; private set; }
    public int? FromTemplateId { get; private set; }

    public int PromotionId { get; private set; }
    public Promotion Promotion { get; private set; }
}