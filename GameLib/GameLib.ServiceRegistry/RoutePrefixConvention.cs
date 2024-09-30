using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace GameLib.ServiceRegistry;

public class RoutePrefixConvention : IApplicationModelConvention
{
    private readonly string _routePrefix;

    public RoutePrefixConvention(string routePrefix)
    {
        _routePrefix = routePrefix;
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        new AttributeRouteModel(new Microsoft.AspNetCore.Mvc.RouteAttribute(_routePrefix)),
                        selector.AttributeRouteModel);
                }
            }
        }
    }
}