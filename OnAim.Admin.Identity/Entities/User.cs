using Microsoft.AspNetCore.Identity;

namespace OnAim.Admin.Identity.Entities
{
    public class User : IdentityUser
    {
        public DateTimeOffset CreateDate { get; set; }
    }
}
