﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class EndpointRepository : IEndpointRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EndpointRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task CheckEndpointHealth()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DisableEndpointAsync(int endpointId)
        {
            var endpoint = await _databaseContext.Endpoints.FindAsync(endpointId);
            if (endpoint != null)
            {
                endpoint.IsEnabled = false;
                await _databaseContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EnableEndpointAsync(int endpointId)
        {
            var endpoint = await _databaseContext.Endpoints.FindAsync(endpointId);
            if (endpoint != null)
            {
                endpoint.IsEnabled = true;
                await _databaseContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<List<EndpointResponseModel>> GetAllEndpoints(RoleFilter roleFilter)
        {
            var query = _databaseContext.Endpoints.AsQueryable();

            if (!string.IsNullOrEmpty(roleFilter.Name))
            {
                query = query.Where(x => x.Name.Contains(roleFilter.Name));
            }

            if (roleFilter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == roleFilter.IsActive);
            }

            if (roleFilter.Type.HasValue)
            {
                query = query.Where(x => x.Type == roleFilter.Type.Value);
            }

            var totalCount = await query.CountAsync();

            var endpoints = await query
                .OrderBy(x => x.Id)
                .Skip((roleFilter.PageNumber - 1) * roleFilter.PageSize)
                .Take(roleFilter.PageSize)
                .ToListAsync();

            var result = endpoints.Select(ep => new EndpointResponseModel
            {
                Id = ep.Id,
                Name = ep.Name,
                Path = ep.Path,
                Description = ep.Description,
                IsEnabled = ep.IsEnabled,
                IsActive = ep.IsActive,
                Type = ToHttpMethod(ep.Type),
                UserId = ep.UserId,
                DateCreated = ep.DateCreated,
                DateUpdated = ep.DateUpdated,
            }).ToList();

            return result;
        }

        public async Task<Endpoint> GetEndpointById(int id)
        {
            var endpoint = await _databaseContext.Endpoints.FirstOrDefaultAsync(x => x.Id == id);

            if (endpoint == null)
            {
                throw new Exception("Not Found");
            }

            var result = new EndpointResponseModel
            {
                Id = endpoint.Id,
                Name = endpoint.Name,
                Path = endpoint.Path,
                Description = endpoint.Description,
                IsActive = endpoint.IsActive,
                IsEnabled = endpoint.IsEnabled,
                Type = ToHttpMethod(endpoint.Type),
                UserId = endpoint.UserId,
                DateCreated = endpoint.DateCreated,
                DateUpdated = endpoint.DateUpdated,
            };
            return endpoint;
        }

        public async Task<IEnumerable<EndpointResponseModel>> GetEndpointsByIdsAsync(IEnumerable<int> ids)
        {
            var endpoint = _databaseContext.Endpoints.Where(r => ids.Contains(r.Id)).AsQueryable();

            var totalCount = await endpoint.CountAsync();

            var eps = await endpoint
                .OrderBy(x => x.Id).ToListAsync();

            var result = eps.Select(x => new EndpointResponseModel
            {
                Name = x.Name,
                Path = x.Path,
                Description = x.Description,
                IsActive = x.IsActive,
                IsEnabled = x.IsEnabled,
                Type = ToHttpMethod(x.Type),
                UserId = x.UserId,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                DateDeleted = x.DateDeleted,
            }).ToList();

            return result;
        }

        public async Task<Endpoint> CreateEndpointAsync(string path, string description = null, string? endpointType = null, int? userId = null)
        {
            var endpoint = await _databaseContext.Endpoints.FirstOrDefaultAsync(e => e.Path == path);

            EndpointType endpointTypeEnum = EndpointType.Get;
            if (endpointType != null && Enum.TryParse(endpointType, true, out EndpointType parsedType))
            {
                endpointTypeEnum = parsedType;
            }

            if (endpoint == null)
            {
                endpoint = new Endpoint
                {
                    Name = path,
                    Path = path,
                    Description = description ?? "Description needed",
                    IsEnabled = true,
                    DateCreated = SystemDate.Now,
                    UserId = userId,
                    IsActive = true,
                    Type = endpointTypeEnum,
                };
                _databaseContext.Endpoints.Add(endpoint);
                await _databaseContext.SaveChangesAsync();
            }
            else if (string.IsNullOrEmpty(endpoint.Description))
            {
                endpoint.Description = "Description needed";
                _databaseContext.Endpoints.Update(endpoint);
                await _databaseContext.SaveChangesAsync();
            }

            return endpoint;
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