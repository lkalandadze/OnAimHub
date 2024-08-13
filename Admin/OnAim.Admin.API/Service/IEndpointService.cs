namespace OnAim.Admin.API.Service
{
    public interface IEndpointService
    {
        List<EndpointInfo> GetAllEndpoints();
        Task SaveEndpointsAsync();
    }
}
