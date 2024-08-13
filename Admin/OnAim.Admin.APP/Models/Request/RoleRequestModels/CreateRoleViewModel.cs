using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Models.Request.Role
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
