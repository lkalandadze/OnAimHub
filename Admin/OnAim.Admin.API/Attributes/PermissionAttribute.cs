using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace OnAim.Admin.API.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _requiredPermission;

        public PermissionAttribute(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var permissions = user.Claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value)
                    .ToList();

                if (permissions.Contains(_requiredPermission))
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}
