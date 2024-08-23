using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.APP.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Models.Response;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApplicationResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllRolesAsync(request.Filter);

            var result = roles.Items.Select(x => new RoleResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                Description = x.Description,
                EndpointGroupModels = x.EndpointGroupModels.Select(xx => new EndpointGroupModeldTO
                {
                    Id = xx.Id,
                    Name = xx.Name,
                    IsActive = xx.IsActive,
                }).ToList()
            }).ToList();

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<RoleResponseModel>
                {
                    PageNumber = roles.PageNumber,
                    PageSize = roles.PageSize,
                    TotalCount = roles.TotalCount,
                    Items = result
                }
            };
        }
    }
}
