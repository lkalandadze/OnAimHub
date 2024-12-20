namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class CreatePromotionView
{
    public string Name { get; set; }
    public string ViewContent { get; set; }
    public int PromotionId { get; set; }
    public string TemplateId { get; set; }
}