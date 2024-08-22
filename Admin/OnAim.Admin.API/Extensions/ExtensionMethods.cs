using System.Security.Claims;

namespace OnAim.Admin.API.Extensions
{
    public static class ExtensionMethods
    {
        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            return user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
        }
    }
}
