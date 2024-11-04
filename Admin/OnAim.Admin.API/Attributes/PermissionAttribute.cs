using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using Microsoft.EntityFrameworkCore;

namespace OnAim.Admin.API.Attributes;

public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
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
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            var filteredRoles = await GetActiveRolesAsync(roles, dbContext);

            if (!filteredRoles.Any())
            {
                context.Result = new ForbidResult();
                return;
            }

            var hasPermission = await permissionService.RolesContainPermission(filteredRoles, dynamicRequiredPermission);

            if (hasPermission)
            {
                return;
            }
        }

        context.Result = new ForbidResult();
    }

    private async Task<List<string>> GetActiveRolesAsync(List<string> roles, DatabaseContext dbContext)
    {
        var activeRoles = new List<string>();

        foreach (var role in roles)
        {
            var roleRecord = await dbContext.Roles
                .Where(x => x.Name == role && x.IsActive && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (roleRecord != null)
            {
                activeRoles.Add(role);
            }
        }

        return activeRoles;
    }
}
