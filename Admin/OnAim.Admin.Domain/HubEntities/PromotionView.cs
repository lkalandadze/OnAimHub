namespace OnAim.Admin.Domain.HubEntities;

public class PromotionView : BaseEntity<int>
{
    public PromotionView()
    {

    }

    public PromotionView(string name, string url, int promotionId, string? templateId = null)
    {
        Name = name;
        Url = url;
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public string Name { get; set; }
    public string Url { get; set; }
    public string? FromTemplateId { get; set; }
    public int? CreatedByUserId { get; set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}