using Microsoft.AspNetCore.Mvc.Controllers;
using OnAim.Admin.APP.Services.Abstract;
using System.Security.Claims;

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

            var dynamicRequiredPermission = $"{controllerName}_{actionName}";


            context.Items["RequiredPermission"] = dynamicRequiredPermission;

            var user = context.User;

            if (user.Identity.IsAuthenticated)
            {
                var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

                foreach (var role in roles)
                {
                    var hasPermission = await permissionService.HasPermissionForRoleAsync(role, dynamicRequiredPermission);

                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
        }
    }
}
