using MediatR;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public class GetEndpointByIdQueryHandler : IRequestHandler<GetEndpointByIdQuery, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public GetEndpointByIdQueryHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointRepository.GetEndpointById(request.Id);

            var result = new EndpointResponseModel
            {
                Id = request.Id,
                Name = endpoint.Name,
                Path = endpoint.Path,
                Description = endpoint.Description,
                IsActive = endpoint.IsActive,
                IsEnabled = endpoint.IsEnabled,
                UserId = endpoint.UserId,
                DateCreated = endpoint.DateCreated,
                DateDeleted = endpoint.DateDeleted,
                DateUpdated = endpoint.DateUpdated,
                Type = ToHttpMethod(endpoint.Type),
            };

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };
        }

        public static string ToHttpMethod(EndpointType? type)
        {
            return type switch
            {
                EndpointType.Get => "GET",
                EndpointType.Create => "POST",
                EndpointType.Update => "PUT",
                EndpointType.Delete => "DELETE",
                _ => "UNKNOWN"
            };
        }
    }
}
