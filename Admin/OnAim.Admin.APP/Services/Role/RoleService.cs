using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;
using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Dtos;
using OnAim.Admin.Contracts.Helpers;
using OnAim.Admin.Contracts.Models;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.APP.Services.Role;

public class RoleService : IRoleService
{
    private readonly IRepository<Admin.Domain.Entities.Role> _roleRepository;
    private readonly IRepository<Admin.Domain.Entities.EndpointGroup> _endpointGroupRepository;
    private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _userRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public RoleService(
        IRepository<OnAim.Admin.Domain.Entities.Role> roleRepository,
        IRepository<OnAim.Admin.Domain.Entities.EndpointGroup> endpointGroupRepository,
        IConfigurationRepository<RoleEndpointGroup> configurationRepository,
        IRepository<OnAim.Admin.Domain.Entities.User> userRepository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _roleRepository = roleRepository;
        _endpointGroupRepository = endpointGroupRepository;
        _configurationRepository = configurationRepository;
        _userRepository = userRepository;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<ApplicationResult> Create(CreateRoleRequest request)
    {
        var existsName = _roleRepository.Query(x => x.Name.ToLower() == request.Name.ToLower()).Any();
        if (existsName)
            throw new BadRequestException("Role With That Name ALready Exists");

        var role = new OnAim.Admin.Domain.Entities.Role(
            request.Name.ToLower(),
            request.Description,
            _securityContextAccessor.UserId
            );

        await _roleRepository.Store(role);
        await _roleRepository.CommitChanges();

        foreach (var group in request.EndpointGroupIds)
        {
            var epgroup = await _endpointGroupRepository.Query(x => x.Id == group).FirstOrDefaultAsync();

            if (epgroup?.IsDeleted == true)
                throw new BadRequestException("EndpointGroup Is Disabled!");

            var roleEndpointGroup = new RoleEndpointGroup(role.Id, epgroup.Id);

            role.RoleEndpointGroups.Add(roleEndpointGroup);
            await _configurationRepository.CommitChanges();
        }

        await _configurationRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Role {role.Name} Successfully Created!",
        };
    }

    public async Task<ApplicationResult> Delete(List<int> ids)
    {
        var roles = await _roleRepository.Query(x => ids.Contains(x.Id)).ToListAsync();

        if (!roles.Any())
            throw new NotFoundException("Role Not Found");

        foreach (var role in roles)
        {
            role.IsActive = false;
            role.IsDeleted = true;
        }

        await _roleRepository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> Update(int id, UpdateRoleRequest request)
    {
        var role = await _roleRepository.Query(x => x.Id == id)
                                 .Include(r => r.RoleEndpointGroups)
                                 .ThenInclude(reg => reg.EndpointGroup)
                                 .ThenInclude(eg => eg.EndpointGroupEndpoints)
                                 .ThenInclude(ege => ege.Endpoint)
                                 .FirstOrDefaultAsync();

        if (role == null)
            throw new NotFoundException("Role not found");

        if (!string.IsNullOrEmpty(request.Name))
        {
            var super = await _roleRepository.Query(x => x.Name == "SuperRole").FirstOrDefaultAsync();

            if (request.Name == super.Name)
                throw new BadRequestException("You don't have permmission to update this role!");

            bool nameExists = await _roleRepository.Query(x => x.Name == request.Name.ToLower() && x.Id != id)
                .AnyAsync();

            if (nameExists)
                throw new BadRequestException("Role with this name already exists.");

            role.Name = request.Name.ToLower();
        }

        if (!request.IsActive)
        {
            role.DateUpdated = SystemDate.Now;
            role.IsActive = false;
        }

        role.Description = request.Description;
        role.DateUpdated = SystemDate.Now;
        role.IsActive = request.IsActive;

        if (request.EndpointGroupIds != null)
        {
            var existingGroups = role.RoleEndpointGroups.Select(reg => new { reg.RoleId, reg.EndpointGroupId }).ToHashSet();

            foreach (var item in request.EndpointGroupIds)
            {
                var group = await _endpointGroupRepository.Query(x => x.Id == item).FirstOrDefaultAsync();

                if (group == null)
                    throw new NotFoundException("Permission Group not found");

                if (!existingGroups.Contains(new { RoleId = role.Id, EndpointGroupId = group.Id }))
                {
                    var roleEndpointGroup = new RoleEndpointGroup(role.Id, group.Id);

                    role.RoleEndpointGroups.Add(roleEndpointGroup);
                }
            }
        }

        await _roleRepository.CommitChanges();
        await _configurationRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Role {role.Name} Successfully Updated!",
        };
    }

    public async Task<ApplicationResult> GetAll(RoleFilter filter)
    {
        var roleQuery = _roleRepository
            .Query(x =>
                     (string.IsNullOrEmpty(filter.Name) || x.Name.ToLower().Contains(filter.Name.ToLower())) &&
                     (!filter.IsActive.HasValue || x.IsActive == filter.IsActive.Value)
        );

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    roleQuery = roleQuery.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    roleQuery = roleQuery.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        if (filter.UserIds != null && filter.UserIds.Any())
            roleQuery = roleQuery.Where(x => x.UserRoles.Any(ur => filter.UserIds.Contains(ur.UserId)));

        if (filter.GroupIds != null && filter.GroupIds.Any())
            roleQuery = roleQuery.Where(x => x.RoleEndpointGroups.Any(ur => filter.GroupIds.Contains(ur.EndpointGroupId)));

        bool sortDescending = filter.SortDescending.GetValueOrDefault();
        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            roleQuery = sortDescending
                ? roleQuery.OrderByDescending(x => x.Id)
                : roleQuery.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "Name" || filter.SortBy == "name")
        {
            roleQuery = sortDescending
                ? roleQuery.OrderByDescending(x => x.Name)
                : roleQuery.OrderBy(x => x.Name);
        }

        var paginatedResult = await Paginator.GetPaginatedResult(
           roleQuery,
           filter,
           role => new RoleShortResponseModel
           {
               Id = role.Id,
               Name = role.Name,
               Description = role.Description,
               IsActive = role.IsActive,
               IsDeleted = role.IsDeleted,
               EndpointGroupModels = role.RoleEndpointGroups
                    .Select(z => new EndpointGroupModeldTO
                    {
                        Id = z.EndpointGroup.Id,
                        Name = z.EndpointGroup.Name,
                        IsActive = z.EndpointGroup.IsActive,
                    }).ToList()
           },
           new List<string> { "Id", "Name" }
       );

        return new ApplicationResult
        {
            Success = true,
            Data = paginatedResult
        };
    }

    public async Task<ApplicationResult> GetById(int id)
    {
        var role = await _roleRepository
          .Query(x => x.Id == id)
          .Include(x => x.UserRoles)
          .ThenInclude(x => x.User)
        .Include(x => x.RoleEndpointGroups)
          .ThenInclude(x => x.EndpointGroup)
        .ThenInclude(x => x.EndpointGroupEndpoints)
          .ThenInclude(x => x.Endpoint)
          .FirstOrDefaultAsync();

        var user = await _userRepository.Query(x => x.Id == role.CreatedBy).FirstOrDefaultAsync();

        if (role == null) { return new ApplicationResult { Success = false, Data = $"Role Not Found!" }; }

        var result = new RoleResponseModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            DateCreated = role.DateCreated,
            UsersResponseModels = role.UserRoles.Select(z => new UserDto
            {
                Id = z.UserId,
                FirstName = z.User.FirstName,
                LastName = z.User.LastName,
                Email = z.User.Email,
            }).ToList(),
            EndpointGroupModels = role.RoleEndpointGroups.Select(x => new EndpointGroupModel
            {
                Id = x.EndpointGroupId,
                Name = x.EndpointGroup.Name,
                IsActive = x.EndpointGroup.IsActive,
                Description = x.EndpointGroup.Description,
                DateCreated = x.EndpointGroup.DateCreated,
                EndpointsCount = x.EndpointGroup.EndpointGroupEndpoints.Count,
                Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new EndpointRequestModel
                {
                    Id = x.EndpointId,
                    Name = x.Endpoint.Name,
                    Description = x.Endpoint.Description,
                    IsActive = x.Endpoint.IsActive,
                    IsEnabled = x.Endpoint.IsDeleted,
                    Type = ToHttpMethodExtension.ToHttpMethod(x.Endpoint.Type),
                    Path = x.Endpoint.Path,
                    DateCreated = x.Endpoint.DateCreated,
                }).ToList(),
            }).ToList(),
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            }
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result,
        };
    }
}