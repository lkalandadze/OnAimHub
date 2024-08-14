namespace OnAim.Admin.API.Service.Endpoint
{
    public interface IEndpointService
    {
        List<EndpointInfo> GetAllEndpoints();
        Task SaveEndpointsAsync();
    }
}
