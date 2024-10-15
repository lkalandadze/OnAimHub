using Microsoft.AspNetCore.Mvc.Controllers;
using OnAim.Admin.API.Extensions;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using System.Text;

namespace OnAim.Admin.API.Middleware;

public class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IPermissionService permissionService, ILogRepository auditLogService, ISecurityContextAccessor securityContextAccessor)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var method = context.Request.Method;
        var path = context.Request.Path;
        var segments = path.Value.Split('/');
        var entityName = segments.Length > 2 ? segments[2] : "Unknown";
        string requestBody;

        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
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
            var hasPermission = await permissionService.RolesContainPermission(roles, dynamicRequiredPermission);

            if (hasPermission)
            {
                await _next(context);
                return;
            }
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Forbidden");

        var logEntry = new AccessDeniedLog(method, securityContextAccessor.UserId, null, requestBody, $"{method} {path} access denied", "Access denied due to insufficient permissions", 0);
        await auditLogService.AddAccessDeniedLogAsync(logEntry);
    }
}
