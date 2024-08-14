using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Shared.Models;
using Microsoft.OpenApi.Extensions;

namespace OnAim.Admin.API.Service.Endpoint
{
    public class EndpointService : IEndpointService
    {
        private readonly DatabaseContext context;

        public EndpointService(DatabaseContext context)
        {
            this.context = context;
        }
        public List<EndpointInfo> GetAllEndpoints()
        {
            var endpoints = new List<EndpointInfo>();

            var controllers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);
           
            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Replace("Controller", "");
                var routePrefix = controller.GetCustomAttribute<RouteAttribute>()?.Template;

                foreach (var method in controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    var httpMethodAttributes = method.GetCustomAttributes<HttpMethodAttribute>(inherit: false).ToList();

                    if (httpMethodAttributes.FirstOrDefault() is { } httpMethod)
                    {
                        var methodName = method.Name;

                        var route = httpMethod.Template;

                        var fullRoute = string.IsNullOrEmpty(routePrefix)
                            ? route
                            : $"{routePrefix}/{route}".Trim('/').Replace("[controller]", controllerName);

                        var httpMethodName = httpMethod.HttpMethods.FirstOrDefault();

                        endpoints.Add(new EndpointInfo
                        {
                            Controller = controller.Name,
                            Name = $"{controllerName}_{methodName}",
                            HttpMethod = httpMethod.HttpMethods.First(),
                            RouteTemplate = fullRoute!
                        });
                    }
                }
            }

            return endpoints;
        }

        public async Task SaveEndpointsAsync()
        {
            var endpoints = GetAllEndpoints();

            var endpointEntities = endpoints.Select(MapToEndpoint).ToList();

            foreach (var endpoint in endpointEntities)
            {
                var existingEndpoint = await context.Endpoints
                    .FirstOrDefaultAsync(e => e.Name == endpoint.Name);

                if (existingEndpoint == null)
                {
                    context.Endpoints.Add(endpoint);
                }
                else
                {
                    existingEndpoint.Name = endpoint.Name;
                    existingEndpoint.Description = endpoint.Description;
                    existingEndpoint.IsEnabled = endpoint.IsEnabled;
                    existingEndpoint.Type = endpoint.Type;
                    context.Endpoints.Update(existingEndpoint);
                }
            }

            await context.SaveChangesAsync();
        }

        private Infrasturcture.Entities.Endpoint MapToEndpoint(EndpointInfo endpointInfo)
        {
            var controllerName = endpointInfo.Controller.EndsWith("Controller")
                                 ? endpointInfo.Controller.Substring(0, endpointInfo.Controller.Length - "Controller".Length)
                                 : endpointInfo.Controller;

            var formattedName = $"{controllerName}/{endpointInfo.Name}";

            return new Infrasturcture.Entities.Endpoint
            {
                Id = Guid.NewGuid().ToString(),
                Name = formattedName,
                Path = formattedName,
                Description = $"Endpoint for {endpointInfo.Name} in {endpointInfo.Controller}",
                IsEnabled = true,
                IsActive = true,
                DateCreated = SystemDate.Now,
                Type = ParseHttpMethodToEndpointType(endpointInfo.HttpMethod),
                UserId = null
            };
        }

        private EndpointType ParseHttpMethodToEndpointType(string httpMethod)
        {
            return httpMethod.ToUpper() switch
            {
                "GET" => EndpointType.Get,
                "POST" => EndpointType.Create,
                "PUT" => EndpointType.Update,
                "DELETE" => EndpointType.Delete,
                _ => throw new ArgumentOutOfRangeException($"Unsupported HTTP method: {httpMethod}")
            };
        }

    }
}
