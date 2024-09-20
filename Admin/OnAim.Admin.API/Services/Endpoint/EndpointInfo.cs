using OnAim.Admin.API.Controllers.Abstract;
using System.Reflection;

namespace OnAim.Admin.API.Service.Endpoint
{
    public class EndpointInfo
    {
        public string Controller { get; set; }
        public string Name { get; set; }
        public string HttpMethod { get; set; }
        public string RouteTemplate { get; set; }
    }
    public static class EntityNames
    {
        public static readonly List<string> All = GetEntityNames();

        private static List<string> GetEntityNames()
        {
            var entityNames = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ApiControllerBase).IsAssignableFrom(type) && type.IsClass)
                .Select(type => type.Name.Replace("Controller", ""))
                .ToList();

            return entityNames;
        }
    }
}
