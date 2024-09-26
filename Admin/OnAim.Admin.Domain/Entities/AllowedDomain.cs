using OnAim.Admin.Domain.Entities.Abstract;

namespace OnAim.Admin.Domain.Entities;

public class AllowedEmailDomain : BaseEntity
{
    public string Domain { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
