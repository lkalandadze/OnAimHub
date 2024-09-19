using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class AllowedEmailDomain : BaseEntity
    {
        public string Domain { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
