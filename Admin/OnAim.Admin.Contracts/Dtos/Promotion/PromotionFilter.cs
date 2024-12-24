using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionFilter : BaseFilter
{
    public string? Name { get; set; }
    public PromotionStatus? Status { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
}
