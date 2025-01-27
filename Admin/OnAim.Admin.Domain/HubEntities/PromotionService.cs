namespace OnAim.Admin.Domain.HubEntities;

//todo: gvanca
public class PromotionService : BaseEntity<int>
{
    public PromotionService()
    {

    }

    public int PromotionId { get; set; }
    public string Type { get; set; }
    public string Service { get; set; }
    public bool IsActive { get; set; }
    public Promotion Promotion { get; private set; }
}
