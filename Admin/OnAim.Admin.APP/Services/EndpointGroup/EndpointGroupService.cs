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

namespace OnAim.Admin.APP.Services.EndpointGroup;

public class EndpointGroupService : IEndpointGroupService
{
    private readonly IRepository<OnAim.Admin.Domain.Entities.EndpointGroup> _repository;
    private readonly IRepository<Admin.Domain.Entities.Endpoint> _endpointRepository;
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _userRepository;
    private readonly IConfigurationRepository<EndpointGroupEndpoint> _endpointGroupEndpointRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public EndpointGroupService(
        IRepository<OnAim.Admin.Domain.Entities.EndpointGroup> repository,
        IRepository<OnAim.Admin.Domain.Entities.Endpoint> endpointRepository,
        IRepository<OnAim.Admin.Domain.Entities.User> userRepository,
        IConfigurationRepository<EndpointGroupEndpoint> endpointGroupEndpointRepository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _repository = repository;
        _endpointRepository = endpointRepository;
        _userRepository = userRepository;
        _endpointGroupEndpointRepository = endpointGroupEndpointRepository;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<ApplicationResult> Create(CreateEndpointGroupRequest model)
    {
        var existedGroupName = await _repository.Query(x => x.Name == model.Name.ToLower()).FirstOrDefaultAsync();

        if (existedGroupName == null || existedGroupName?.IsDeleted == true)
        {
            var endpointGroup = OnAim.Admin.Domain.Entities.EndpointGroup.Create(
                model.Name.ToLower(),
                model.Description,
                _securityContextAccessor.UserId,
            new List<EndpointGroupEndpoint>()
                );

            foreach (var endpointId in model.EndpointIds)
            {
                var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

                if (endpoint?.IsDeleted == true)
                {
                    throw new BadRequestException("Permmission Is Disabled!");
                }

                var endpointGroupEndpoint = new EndpointGroupEndpoint(endpointGroup.Id, endpoint.Id);

                endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
            }

            await _repository.Store(endpointGroup);
            await _repository.CommitChanges();
        }
        else
        {
            throw new BadRequestException("Permmission Group with that name already exists!");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission Group {model.Name} Successfully Created",
        };
    }

    public async Task<ApplicationResult> Delete(List<int> ids)
    {
        var groups = await _repository.Query(x => ids.Contains(x.Id)).ToListAsync();

        if (!groups.Any())
            throw new NotFoundException("Permission Group Not Found");

        foreach (var group in groups)
        {
            group.IsActive = false;
            group.IsDeleted = true;
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> Update(int id, UpdateEndpointGroupRequest model)
    {
        var group = await _repository
            .Query(x => x.Id == id)
            .Include(g => g.EndpointGroupEndpoints)
            .FirstOrDefaultAsync();

        if (group == null)
            throw new BadRequestException("Permmission Group Not Found");

        if (!string.IsNullOrEmpty(model.Name))
        {
            var super = await _repository.Query(x => x.Name == "SuperGroup").FirstOrDefaultAsync();

            if (model.Name == super.Name)
                throw new BadRequestException("You don't have permmission to update this group!");
            bool nameExists = await _repository.Query(x => x.Name == model.Name.ToLower() && x.Id != id)
            .AnyAsync();

            if (nameExists)
                throw new BadRequestException("Permmission Group with this name already exists.");

            group.Name = model.Name.ToLower();
        }

        if (model.Description != null)
            group.Description = model.Description;

        if (model.IsActive.HasValue)
            group.IsActive = model.IsActive.Value;

        var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
        var newEndpointIds = model.EndpointIds ?? new List<int>();

        var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
        var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

        foreach (var endpointId in endpointsToAdd)
        {
            var endpoint = await _endpointRepository.Query(x => x.Id == endpointId).FirstOrDefaultAsync();

            if (endpoint != null)
            {
                var alreadyExists = group.EndpointGroupEndpoints.Any(ege => ege.EndpointId == endpointId);
                if (!alreadyExists)
                {
                    await _endpointGroupEndpointRepository.Store(new EndpointGroupEndpoint(group.Id, endpoint.Id));
                }
            }
        }

        foreach (var endpointId in endpointsToRemove)
        {
            var endpointGroupEndpoint = await _endpointGroupEndpointRepository.Query(x => x.EndpointGroupId == group.Id && x.EndpointId == endpointId)
                .FirstOrDefaultAsync();

            if (endpointGroupEndpoint != null)
                await _endpointGroupEndpointRepository.Remove(endpointGroupEndpoint);
        }

        group.DateUpdated = SystemDate.Now;

        await _endpointGroupEndpointRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission Group {model.Name} Successfully Updated",
        };
    }

    public async Task<ApplicationResult> GetAll(EndpointGroupFilter filter)
    {
        var query = _repository.Query(x =>
        (string.IsNullOrEmpty(filter.Name) || x.Name.ToLower().Contains(filter.Name.ToLower())) &&
                     (!filter.IsActive.HasValue || x.IsActive == filter.IsActive.Value)
        );

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    query = query.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    query = query.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        if (filter.RoleIds != null && filter.RoleIds.Any())
            query = query.Where(x => x.RoleEndpointGroups.Any(ur => filter.RoleIds.Contains(ur.RoleId)));

        if (filter.EndpointIds != null && filter.EndpointIds.Any())
            query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => filter.EndpointIds.Contains(ur.EndpointId)));

        bool sortDescending = filter.SortDescending.GetValueOrDefault();
        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "Name" || filter.SortBy == "name")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name);
        }

        var paginatedResult = await Paginator.GetPaginatedResult(
            query,
            filter,
            item => new EndpointGroupModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                DateUpdated = item.DateUpdated,
                DateCreated = item.DateCreated,
                DateDeleted = item.DateDeleted,
                EndpointsCount = item.EndpointGroupEndpoints.Count,
                Endpoints = item.EndpointGroupEndpoints.Select(xx => new EndpointRequestModel
                {
                    Id = xx.Endpoint.Id,
                    Name = xx.Endpoint.Name,
                    Path = xx.Endpoint.Path,
                    Description = xx.Endpoint.Description,
                    Type = ToHttpMethodExtension.ToHttpMethod(xx.Endpoint.Type),
                    IsActive = xx.Endpoint.IsActive,
                    DateCreated = xx.Endpoint.DateCreated,
                }).ToList(),
                IsActive = item.IsActive,
                IsDeleted = item.IsDeleted,
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
        var group = await _repository
           .Query(x => x.Id == id)
           .Include(x => x.RoleEndpointGroups)
           .ThenInclude(x => x.Role)
           .Include(x => x.EndpointGroupEndpoints)
           .ThenInclude(x => x.Endpoint)
           .FirstOrDefaultAsync();

        var user = await _userRepository.Query(x => x.Id == group.CreatedBy).FirstOrDefaultAsync();

        if (group == null)
            throw new NotFoundException("Permmission Group Not Found!");

        var res = new EndpointGroupResponseDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            IsActive = group.IsActive,
            DateCreated = group.DateCreated,
            DateDeleted = group.DateDeleted,
            DateUpdated = group.DateUpdated,
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            },
            Roles = group.RoleEndpointGroups.Select(z => new RoleDto
            {
                Id = z.Role.Id,
                Name = z.Role.Name,
                IsActive = z.Role.IsActive,
            }).ToList(),
            Endpoints = group.EndpointGroupEndpoints.Select(x => new EndpointModel
            {
                Id = x.Endpoint.Id,
                Name = x.Endpoint.Name,
                Description = x.Endpoint.Description,
                IsActive = x.Endpoint.IsActive,
                IsEnabled = x.Endpoint.IsDeleted,
            }).ToList()
        };

        return new ApplicationResult
        {
            Success = true,
            Data = res,
        };
    }
}
