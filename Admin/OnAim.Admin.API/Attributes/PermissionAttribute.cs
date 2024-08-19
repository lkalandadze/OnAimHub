using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Services.Abstract;
using System.Security.Claims;

namespace OnAim.Admin.API.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
               .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
               .Any();

            if (allowAnonymous)
            {
                return;
            }

            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var dynamicRequiredPermission = $"{controllerName}_{actionName}";

            context.HttpContext.Items["RequiredPermission"] = dynamicRequiredPermission;


            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

                var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

                foreach (var role in roles)
                {
                    var hasPermission = permissionService.HasPermissionForRoleAsync(role, dynamicRequiredPermission).Result;

                    if (hasPermission)
                    {
                        return;
                    }
                }
            }

            context.Result = new ForbidResult();
        }
    }
}
