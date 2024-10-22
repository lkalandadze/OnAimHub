using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Endpoint = OnAim.Admin.Domain.Entities.Endpoint;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;

namespace OnAim.Admin.API.Attributes;

public class CheckEndpointStatusAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var dynamicRequiredPermission = $"{actionName}_{controllerName}";

        var dbContext = (DatabaseContext)context.HttpContext.RequestServices.GetService(typeof(DatabaseContext));
        var endpoints = await GetEndpointById(dynamicRequiredPermission, dbContext);

        if (endpoints == null || endpoints.IsDeleted || !endpoints.IsActive)
        {
            context.Result = new NotFoundResult();
            return;
        }

        await next();
    }

    private async Task<Endpoint> GetEndpointById(string endpoint, DatabaseContext context)
    {
        return await context.Endpoints.Where(x => x.Name == endpoint).FirstOrDefaultAsync();
    }
}
