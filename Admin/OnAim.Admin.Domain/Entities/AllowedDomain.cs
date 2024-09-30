using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class AllowedEmailDomain : BaseEntity
{
    public string Domain { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsDeleted { get; set; }

    public AllowedEmailDomain(string domain, int? createdBy)
    {
        Domain = domain;
        CreatedBy = createdBy;
        IsDeleted = false;
        DateCreated = SystemDate.Now;
        IsActive = true;
    }

    public void Update(string domain, bool isActive)
    {
        Domain = domain;
        IsActive = isActive;
        DateUpdated = SystemDate.Now;
    }
}
