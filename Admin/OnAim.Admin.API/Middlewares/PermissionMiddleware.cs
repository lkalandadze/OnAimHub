using Microsoft.AspNetCore.Mvc.Controllers;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.API.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IPermissionService permissionService)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var allowAnonymous = endpoint.Metadata.OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            var controllerActionDescriptor = endpoint.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();
            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;

            var dynamicRequiredPermission = $"{actionName}_{controllerName}";

            context.Items["RequiredPermission"] = dynamicRequiredPermission;

            var user = context.User;

            if (user.Identity.IsAuthenticated)
            {
                var roles = user.GetRoles();

                foreach (var role in roles)
                {
                    if (role == "SuperRole")
                    {
                        await _next(context);
                        return;
                    }
                }

                var hasPermission = await permissionService.RolesContainPermission(roles, dynamicRequiredPermission);

                if (hasPermission)
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
        }
    }
}
