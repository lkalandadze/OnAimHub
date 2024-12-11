namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionServiceDto
{
    public int PromotionId { get; set; }
    public string Type { get; set; }
    public string Service { get; set; }
    public bool IsActive { get; set; }
    public PromotionDto Promotion { get; private set; }
}
