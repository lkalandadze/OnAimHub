using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.API.Service
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
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(method => method.GetCustomAttributes<HttpMethodAttribute>().Any());

                foreach (var action in actions)
                {
                    var routeAttributes = action.GetCustomAttributes<RouteAttribute>().ToList();
                    var httpMethodAttributes = action.GetCustomAttributes<HttpMethodAttribute>().ToList();

                    var routeTemplate = routeAttributes.FirstOrDefault()?.Template ?? "No Route";
                    var httpMethod = httpMethodAttributes.FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "No HTTP Method";

                    if (routeAttributes.Count > 1)
                    {
                        routeTemplate = string.Join(", ", routeAttributes.Select(attr => attr.Template));
                    }
                    else if (routeTemplate == "No Route")
                    {
                        routeTemplate = httpMethodAttributes
                            .Select(attr => attr.HttpMethods.FirstOrDefault())
                            .FirstOrDefault() ?? "No Route";
                    }

                    endpoints.Add(new EndpointInfo
                    {
                        Controller = controller.Name,
                        Action = action.Name,
                        HttpMethod = httpMethod,
                        RouteTemplate = routeTemplate
                    });
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

            var formattedName = $"{controllerName}/{endpointInfo.Action}";

            return new Infrasturcture.Entities.Endpoint
            {
                Id = Guid.NewGuid().ToString(),
                Name = formattedName,
                Path = formattedName,
                Description = $"Endpoint for {endpointInfo.Action} in {endpointInfo.Controller}",
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
