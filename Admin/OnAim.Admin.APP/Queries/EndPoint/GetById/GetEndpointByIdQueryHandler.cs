using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public class GetEndpointByIdQueryHandler : IRequestHandler<GetEndpointByIdQuery, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;

        public GetEndpointByIdQueryHandler(IRepository<Endpoint> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
        {
            var endpoint = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

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
                Type = Infrasturcture.Extensions.ToHttpMethodExtension.ToHttpMethod(endpoint.Type),
            };

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };
        }
    }
}
