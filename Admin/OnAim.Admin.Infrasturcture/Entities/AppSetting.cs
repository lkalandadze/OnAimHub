using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class AppSetting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
