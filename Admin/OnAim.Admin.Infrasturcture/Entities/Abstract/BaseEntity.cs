using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Infrasturcture.Entities.Abstract
{
    public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
    }
}
