using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OnAim.Admin.APP.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User ID claim is not present or is not a valid integer");
        }
    }
}
