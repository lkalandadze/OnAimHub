﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Services.Endpoint;

public class EndpointService : IEndpointService
{
    private readonly IRepository<Admin.Domain.Entities.Endpoint> _repository;
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _userRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public EndpointService(
        IRepository<OnAim.Admin.Domain.Entities.Endpoint> repository,
        IRepository<OnAim.Admin.Domain.Entities.User> userRepository,
        ISecurityContextAccessor securityContextAccessor)
    {
        _repository = repository;
        _userRepository = userRepository;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<ApplicationResult> Create(List<CreateEndpointDto> endpoints)
    {
        var results = new List<OnAim.Admin.Domain.Entities.Endpoint>();

        foreach (var endpointDto in endpoints)
        {
            var existedEndpoint = await _repository.Query(x => x.Name == endpointDto.Name).FirstOrDefaultAsync();

            EndpointType endpointTypeEnum = EndpointType.Get;

            if (endpointDto.Type != null && Enum.TryParse(endpointDto.Type, true, out EndpointType parsedType))
                endpointTypeEnum = parsedType;

            if (existedEndpoint != null)
                throw new BadRequestException($"Endpoint with name '{endpointDto.Name}' already exists.");

            var endpoint = new OnAim.Admin.Domain.Entities.Endpoint(
                endpointDto.Name,
                endpointDto.Name,
                _securityContextAccessor.UserId,
                endpointTypeEnum,
                endpointDto.Description ?? "Description needed"
            );

            await _repository.Store(endpoint);
            results.Add(endpoint);
        }

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = string.Join(", ", results.Select(x => x.Name)),
        };
    }

    public async Task<ApplicationResult> Delete(List<int> ids)
    {
        var endpoints = await _repository.Query(x => ids.Contains(x.Id)).ToListAsync();

        if (!endpoints.Any())
            throw new NotFoundException("Permission Not Found");

        foreach (var endpoint in endpoints)
        {
            endpoint.IsActive = false;
            endpoint.IsDeleted = true;
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> Update(int id, UpdateEndpointDto endpoint)
    {
        var ep = await _repository.Query(x => x.Id == id).FirstOrDefaultAsync();

        if (ep == null || ep?.IsDeleted == true)
            throw new NotFoundException("Permission Not Found");

        ep.Description = endpoint.Description;
        ep.IsActive = endpoint.IsActive ?? true;

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission {ep.Name} Successfully Updated"
        };
    }

    public async Task<ApplicationResult> GetAll(EndpointFilter filter)
    {
        var query = _repository
        .Query(x =>
        (string.IsNullOrEmpty(filter.Name) || x.Name.ToLower().Contains(filter.Name.ToLower())) &&
        (!filter.IsActive.HasValue || x.IsActive == filter.IsActive.Value) &&
                    (!filter.Type.HasValue || x.Type == filter.Type.Value)
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

        if (filter.GroupIds != null && filter.GroupIds.Any())
            query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => filter.GroupIds.Contains(ur.EndpointGroupId)));

        var paginatedResult = await Paginator.GetPaginatedResult(
            query,
            filter,
            item => new EndpointResponseModel
            {
                Id = item.Id,
                Path = item.Path,
                Name = item.Name,
                Description = item.Description,
                Type = ToHttpMethodExtension.ToHttpMethod(item.Type),
                IsActive = item.IsActive,
                IsDeleted = item.IsDeleted,
                DateCreated = item.DateCreated,
                DateUpdated = item.DateUpdated,
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
        var endpoint = await _repository
           .Query(x => x.Id == id)
           .Include(x => x.EndpointGroupEndpoints)
           .ThenInclude(x => x.EndpointGroup)
           .FirstOrDefaultAsync();

        if (endpoint == null)
            throw new NotFoundException("Permmission Not Found!");

        var user = await _userRepository.Query(x => x.Id == endpoint.CreatedBy).FirstOrDefaultAsync();

        var result = new EndpointResponseModel
        {
            Id = endpoint.Id,
            Name = endpoint.Name,
            Path = endpoint.Path,
            Description = endpoint.Description,
            IsActive = endpoint.IsActive,
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            },
            DateCreated = endpoint.DateCreated,
            DateDeleted = endpoint.DateDeleted,
            DateUpdated = endpoint.DateUpdated,
            Type = ToHttpMethodExtension.ToHttpMethod(endpoint.Type),
            Groups = endpoint.EndpointGroupEndpoints.Select(x => new EndpointGroupDto
            {
                Id = x.EndpointGroupId,
                Name = x.EndpointGroup.Name,
                IsActive = x.EndpointGroup.IsActive,
            }).ToList(),
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result,
        };
    }
}
