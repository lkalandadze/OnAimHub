using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.API.Extensions;

namespace OnAim.Admin.API.Attributes;

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
        var dynamicRequiredPermission = $"{actionName}_{controllerName}";

        context.HttpContext.Items["RequiredPermission"] = dynamicRequiredPermission;

        var user = context.HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            var roles = user.GetRoles();

            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

            var hasPermission = await permissionService.RolesContainPermission(roles, dynamicRequiredPermission);

            if (hasPermission)
            {
                return;
            }
        }

        context.Result = new ForbidResult();
    }
}
